<template>
  <v-container fluid>
    <mobile-header :title="pageTitle">
      <template>
        <v-btn icon large v-on:click="refresh()">
          <v-icon>refresh</v-icon>
        </v-btn>
      </template>
    </mobile-header>

    <v-card >
      <v-toolbar id="stations-toolbar" dense>
        <v-text-field class="ml-2" clearable v-model="searchKey"></v-text-field>
        <v-btn icon v-on:click="search()">
          <v-icon>search</v-icon>
        </v-btn>
        <v-divider vertical class="mr-1"></v-divider>        
        <v-menu close-delay="500" bottom left>
          <v-btn slot="activator" icon>
            <v-icon light class="ml-3 mr-3">{{ displayMode === 'map'? 'place' : 'format_list_bulleted' }}</v-icon>
          </v-btn>
          <v-list>
            <v-list-tile @click.native="changeDisplayMode('map')">
              <v-list-tile-action
                v-bind:style="{visibility: displayMode == 'map' ? 'visible' : 'hidden'}"
              >
                <v-icon class="mr-3">check</v-icon>
              </v-list-tile-action>
              <v-list-tile-title>地图模式</v-list-tile-title>
              <v-list-tile-action>
                <v-icon>place</v-icon>
              </v-list-tile-action>
            </v-list-tile>
            <v-list-tile @click.native="changeDisplayMode('list')">
              <v-list-tile-action
                v-bind:style="{visibility: displayMode == 'list' ? 'visible' : 'hidden'}"
              >
                <v-icon class="mr-3">check</v-icon>
              </v-list-tile-action>
              <v-list-tile-title>列表模式</v-list-tile-title>
              <v-list-tile-action>
                <v-icon>format_list_bulleted</v-icon>
              </v-list-tile-action>
            </v-list-tile>            
          </v-list>
        </v-menu>
        <v-dialog v-model="filterMenu" persistent max-width="280px">
          <v-btn slot="activator" icon style="font-size:18px">
            <v-icon light>filter_list</v-icon>
          </v-btn>
          <v-card class="elevation-20">           
            
            <v-spacer></v-spacer>
            <v-divider></v-divider>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn flat @click="filterCancelClick()">取消</v-btn>
              <v-btn color="primary" flat @click="filterApplyClick()">应用</v-btn>
            </v-card-actions>
          </v-card>
          <v-spacer></v-spacer>
        </v-dialog>
      </v-toolbar>
      <v-layout justify-center align-center v-show="displayMode !== 'map'">
        <v-layout v-show="this.stations.length === 0" justify-center>
          <p class="headline mt-5">没有数据</p>
        </v-layout>
        <v-list two-line>
          <template v-for="(st, index) in stations">
            <v-list-tile
              :key="st.id"
              avatar
              @click.native="showDetail(st)"
              class="stationItem"
            >
              <v-list-tile-action>
                <v-icon v-if="st.warning" color="red" class="hot_icon">whatshot</v-icon>
              </v-list-tile-action>
              
              <v-list-tile-content>
                <v-list-tile-sub-title class="text--primary">
                  <v-icon class="title_icon" color="red">place</v-icon>
                  {{ st.location }}
                </v-list-tile-sub-title>
                <v-list-tile-sub-title>
                  <i class="cus-icon slot">&nbsp;&nbsp;&nbsp;&nbsp;</i>
                  <span class="disable_pn">{{st.machineNumber}}</span>
                </v-list-tile-sub-title>
              </v-list-tile-content>

              <v-list-tile-action>
                <v-list-tile-action-text>
                  <v-layout align-center>
                    <v-flex>
                      <div class="text--primary">
                        <v-icon class="amount_icon" color="#ffd700">fas fa-coins</v-icon>
                        {{ parseInt(st.datacount).toLocaleString() }}
                      </div>
                      <div>
                        <v-icon class="amount_icon">fas fa-calculator</v-icon>
                        {{ parseInt(st.warnaccount).toLocaleString() }}
                      </div>
                    </v-flex>
                  </v-layout>
                </v-list-tile-action-text>
              </v-list-tile-action>
            </v-list-tile>
            <v-divider v-if="index + 1 < stations.length" :key="`divider-${index}`"></v-divider>
          </template>
        </v-list>
        <v-layout v-show="hasMoreData" justify-center>
          <v-btn small flat block color="white" v-on:click="showMore()">
            <v-icon medium color="indigo">expand_more</v-icon>
          </v-btn>
        </v-layout>
      </v-layout>
      <v-layout v-show="displayMode === 'map'">
        <div id="mapContainer" style="width:100vw; height:100vh"></div>
      </v-layout>
    </v-card>
  </v-container>
</template>

<script>
import _ from "lodash";
import { TMap } from "../modules/tmap";

export default {
  name: "Stations",
  data() {
    return {
      pageTitle: "基站",
      displayMode: "map",
      searchKey: "",

      refreshFlag: true,
      refreshInterval: 15,
      pageSize: 30,

      filterMenu: false,
      stations: []
    };
  },
  computed: {
    hasMoreData: {
      get() {
        return true;
      }
    }
  },
  methods: {
    search() {
      console.log("search:" + this.searchKey);
    },
    changeDisplayMode(mode) {
      this.displayMode = mode;
      console.log(`display mode changed to ${this.displayMode}`);
    },

    isBlank(val) {
      if (val === null) return true;

      if (typeof val === "number") return false;

      if (typeof val === "string") {
        return val.trim().length === 0;
      }

      if (val === undefined) return true;
      if (typeof val === "boolean") return false;

      return true;
    },

    showDetail(station) {
      this.$router.push({
        name: "stationDetail",
        params: { station: station }
      });
    },
    refresh() {
      console.log("refresh");
      this.loadStations();
      window.scrollTo(0, 0);
    },
    showFirstPage() {
      if (this.stations.length <= this.pageSize) {
        this.stations = this.stations;
      } else {
        this.stations = this.stations.slice(0, this.pageSize);
      }
    },
    showMore() {
      this.$utils.toast("没有更多");
    },

    loadStations() {
      this.$utils.showLoading();
      this.$http
        .get("/api/stations/list")
        .then(result => {
          this.$utils.hideLoading();
          var retData = result.data;
          this.stations = _.orderBy(retData, [], []);
          this.showFirstPage();
        })
        .catch(error => {
          this.$utils.hideLoading();
          var errorMsg = "";
          if (
            error &&
            error.response &&
            error.response.status &&
            error.response.status === 401
          ) {
            errorMsg = "获取站点数据超时";
          } else {
            errorMsg = `获取站点数据时出错: ${error.message}`;
          }

          this.$utils.toast(errorMsg);
        });
    },
    filterCancelClick() {
      this.filterMenu = false;
    },
    filterApplyClick() {
      this.filterMenu = false;
    },
    initMap() {
      var buildings = new AMap.Buildings({
          'zooms':[16,18],
          'zIndex':10,
          'heightFactor':2
      });
      var map = new AMap.Map('mapContainer', {
          zoom:11,
          center: [116.397428, 39.90923],
          layers: [
              //new AMap.TileLayer.Satellite(),
              //buildings
          ],
          viewMode:'2D'
      });
      var marker = new AMap.Marker({
          position:[116.39, 39.9]//位置
      })
      map.add(marker);
      marker.on('click',(e) => {
        var infoWindow = new AMap.InfoWindow({
            isCustom: true,
            content:'<div>信息窗体</div>',
            offset: new AMap.Pixel(16, -45)
        });
        infoWindow.open(map, e.target.getPosition());
      });
      
      AMap.plugin('AMap.Geolocation', function() {
        var geolocation = new AMap.Geolocation({
          enableHighAccuracy: true,
          timeout: 10000,
          buttonOffset: new AMap.Pixel(10, 170),
          zoomToAccuracy: true,     
          buttonPosition: 'RB'
        });

        map.addControl(geolocation);
        geolocation.getCurrentPosition(function(status,result){
            if(status=='complete'){
                console.log(JSON.stringify(result));
            }else{
                console.error(JSON.stringify(result));
            }
        });
      });

    //   TMap("CFDBZ-2RSRU-GBOVG-4HHQH-WKXQ2-G7BVR").then(qq => {
    //     console.log("start init qq map");
    //     let myLatlng = new qq.maps.LatLng(39.916527, 116.397128);
    //     let myOptions = {
    //       zoom: 12, //设置地图缩放级别
    //       center: myLatlng, //设置中心点样式
    //       mapTypeId: qq.maps.MapTypeId.ROADMAP, //设置地图样式详情参见MapType,
    //       mapStyleId: "style1"
    //     };
    //     let map = new qq.maps.Map(
    //       document.getElementById("mapContainer"),
    //       myOptions
    //     );        
    //     qq.maps.event.addListener(map, "click", function(event) {
    //       console.log(
    //         "您点击的位置为: [" +
    //           event.latLng.getLat() +
    //           ", " +
    //           event.latLng.getLng() +
    //           "]"
    //       );
    //     });

    //     var geolocation = new qq.maps.Geolocation("CFDBZ-2RSRU-GBOVG-4HHQH-WKXQ2-G7BVR", "BSM");
    //     geolocation.getLocation(position => {
    //       let pos = new qq.maps.LatLng(position.lat, position.lng);
    //       map.panTo(pos);
    //         var marker = new qq.maps.Marker({
    //             position: pos,
    //             animation: qq.maps.MarkerAnimation.BOUNCE,
    //             map: map
    //         });

    //         var citylocation = new qq.maps.CityService({
    //             map : map,
    //             complete : function(results){
    //                 console.log(9999,results)
    //             }
    //         });
    //     }, (err) => {
    //       console.error('定位失败', err);
    //     }, {timeout: 8000});
                      
    // });
    }
  },
  mounted: function() {
    this.initMap();
  },
  beforeMount: function() {},
  activated: function() {
    //this.initMap();
    this.loadStations();
  },
  deactivated: function() {}
};
</script>

<style scoped>
.container {
  padding-left: 0;
  padding-right: 0;
}
@media only screen and (max-width: 959px) {
  .container {
    padding-left: 0;
    padding-right: 0;
  }
}
#stations-toolbar >>> .v-toolbar__content {
  padding: 0px 8px 0px 8px !important;
}
.total_head {
  color: rgba(0, 0, 0, 0.54);
}
.stationItem >>> .v-list__tile {
  padding: 0px 8px 0px 6px !important;
}
.v-list__tile__avatar {
  min-width: 46px;
}
.v-list__tile__action {
  min-width: 16px;
}
i.hot_icon {
  font-size: 14px;
}
i.title_icon {
  font-size: 17px;
}
i.item_icon {
  font-size: 14px;
  color: rgba(0, 0, 0, 0.54);
  padding: 1px;
}
.amount_icon {
  font-size: 12px;
}
.totalArea {
  min-width: 54px;
  color: rgba(0, 0, 0, 0.54);
}
.cus-icon {
  position: relative;
  z-index: 1;
}
.cus-icon::before {
  content: "";
  position: absolute;
  z-index: -1;
  top: 0;
  left: 0;
  background-repeat: no-repeat;
}

#filter_toolbar_dialog {
  max-width: 290px;
}
.filter_toolbar_item {
  max-width: 270px;
}
.filter_toolbar_item >>> .v-list__tile {
  height: 40px !important;
}
.v-text-field--single-line >>> .v-input__slot {
  height: 36px;
  min-height: 36px;
}
.v-text-field--single-line >>> .v-input__control {
  min-height: 36px;
}
.v-text-field--single-line >>> input {
  margin-top: 0;
}
.filter_toolbar_item >>> .v-select__selections {
  flex-wrap: nowrap;
}
</style>
