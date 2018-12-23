<template>
  <v-container fluid style="padding: 0">
    <mobile-header :title="pageTitle">
      <template>
        <v-btn icon large v-on:click="refresh()">
          <v-icon>refresh</v-icon>
        </v-btn>
      </template>
    </mobile-header>

    <v-card >
      <v-toolbar id="stations-toolbar" dense>
        <v-text-field id="searchKey" class="ml-2" clearable v-model="searchKey"></v-text-field>        
        <v-dialog v-model="cityDialog" scrollable persistent max-width="85vw">
          <v-btn slot="activator" icon style="font-size:18px" v-on:click="selectCity">
            <v-icon light>location_city</v-icon>
          </v-btn>          
          <v-card class="elevation-20 px-2">
            <v-card-text style="">
              <v-list two-line subheader dense>
                <v-subheader>省</v-subheader>
                <v-list-tile class="filter_toolbar_item">
                  <v-list-tile-action>
                    <v-select
                      :items="provinces"
                      item-text="name"
                      v-model="curProvince"
                      return-object
                      v-on:change="changeProvince"
                    ></v-select>
                  </v-list-tile-action>
                </v-list-tile>
              </v-list>
              <v-list two-line subheader dense>
                <v-subheader>市</v-subheader>
                <v-list-tile class="filter_toolbar_item">
                  <v-list-tile-action>
                    <v-select
                      :items="cities"
                      item-text="name"
                      v-model="curCity"
                      return-object
                    ></v-select>
                  </v-list-tile-action>
                </v-list-tile>
              </v-list>
            </v-card-text>
            <v-divider></v-divider>   
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn flat @click="cityDialog = false">取消</v-btn>
              <v-btn color="primary" flat v-on:touchstart="applyCity()">选择</v-btn>
            </v-card-actions>
          </v-card>
          <v-spacer></v-spacer>
        </v-dialog>
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
            <v-card-text style="height: 300px; padding: 12px 0 0 0;">
              <v-layout row justify-space-between wrap >
                <v-flex xs12 sm12 pa-3>
                  <v-text-field
                    v-model="filter.id"
                    hide-details
                    label="基站ID"
                    type="number"
                    maxlength="16"
                  ></v-text-field>
                </v-flex>
                <v-flex xs12 sm12 pa-3>
                  <v-text-field
                    v-model="filter.title"
                    hide-details
                    label="基站名称"
                    type="number"
                    maxlength="16"
                  ></v-text-field>
                </v-flex>
                <v-flex xs12 sm12 pa-3>
                  <v-select
                      v-model="filter.levels"
                      clearable
                      chips
                      small-chips
                      single-line
                      multiple
                      hide-details
                      :items="stLevels"
                      label="基站级别"
                    >
                      <template slot="selection" slot-scope="{ item, index }">
                        <v-chip small v-if="index === 0">
                          <span>{{ item }}</span>
                        </v-chip>
                        <v-chip small v-if="index === 1">
                          <span>{{ item }}</span>
                        </v-chip>
                        <span
                          v-if="index === 2"
                          class="grey--text caption"
                        >+{{ filter.levels.length - 1 }}</span>
                      </template>
                    </v-select>
                </v-flex>
              </v-layout>
            </v-card-text>
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
      <v-layout row wrap justify-center align-center v-show="displayMode !== 'map'">
        <v-flex xs12>
          <v-layout v-show="this.stations.length === 0" justify-center>
            <p class="headline mt-5">没有数据</p>
          </v-layout>
        </v-flex>
        <v-flex xs12>
            <v-list two-line>
              <template v-for="(st, index) in stations">
                <v-list-tile                  
                  :key="st.id"
                  avatar
                  @click.native="showDetail(st)"
                  class="stationItem"
                >
                  <v-list-tile-avatar>
                    <v-icon v-if="st.warns > 0" color="red">priority_high</v-icon>
                  </v-list-tile-avatar>
                  
                  <v-list-tile-content>
                    <v-list-tile-title class="text--primary">
                      <v-icon class="title_icon">place</v-icon>
                      {{ st.title }}
                    </v-list-tile-title>
                    <v-list-tile-sub-title>
                      <span class="disable_pn">{{st.id}}</span>
                    </v-list-tile-sub-title>
                  </v-list-tile-content>

                  <v-list-tile-action>
                    <v-btn icon ripple>
                      <v-icon small>navigate_next</v-icon>
                    </v-btn>
                  </v-list-tile-action>                     
                </v-list-tile>
                <v-divider v-if="index + 1 < stations.length" :key="`divider-${index}`"></v-divider>
              </template>  
            </v-list>
        </v-flex>
        <v-flex xs12>
          <v-layout v-show="hasMoreData" justify-center>
            <v-btn small flat block color="white" v-on:click="showMore()">
              <v-icon medium color="indigo">expand_more</v-icon>
            </v-btn>
          </v-layout>
        </v-flex>        
      </v-layout>
      <v-layout v-show="displayMode === 'map'">
        <!-- <el-amap-search-box class="search-box" :search-option="mapSearchOption" :on-search-result="onMapSearchResult"></el-amap-search-box> -->
        <el-amap ref="map" vid="mapContainer" :amap-manager="amapManager" :center="mapCenter" :zoom="mapZoom" :plugin="mapPlugins" :events="mapEvents" class="map">
          <el-amap-marker v-for="(marker, index) in markers" 
            :position="marker.position" 
            :events="marker.events" 
            :visible="marker.visible" 
            :draggable="marker.draggable" 
            :vid="index"
            v-bind:key="index"
            :label="marker.label">
          </el-amap-marker>
          <el-amap-marker  
            :position="tempMarker.position" 
            :events="tempMarker.events" 
            :visible="tempMarker.visible" 
            :draggable="tempMarker.draggable" 
            animation="AMAP_ANIMATION_DROP"
            clickable="true"
            >
          </el-amap-marker>
        </el-amap>
        <v-menu
          v-model="map_temp_menu_show"
          :position-x="map_temp_menu_x"
          :position-y="map_temp_menu_y"
          absolute
          offset-y
          style="z-index: 999"
        >
          <v-list>
            <v-list-tile>
              <v-list-tile-title @click="onAddNewStationClicked">增加新基站</v-list-tile-title>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-title @click="cancelTempMarker">取消操作</v-list-tile-title>
            </v-list-tile>
          </v-list>
        </v-menu>
        <v-menu
          v-model="map_station_menu_show"
          :position-x="map_station_menu_x"
          :position-y="map_station_menu_y"
          absolute
          offset-y
          style="z-index: 999"
        >
          <v-list>
            <v-list-tile>
              <v-list-tile-title @click="onViewStationMenuClicked">查看基站数据</v-list-tile-title>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-title @click="onEditStationMenuClicked">编辑基站信息</v-list-tile-title>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-title @click="onEditStationMenuClicked">移动基站位置</v-list-tile-title>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-title @click="onAddCommentMenuClicked">增加备注</v-list-tile-title>
            </v-list-tile>
          </v-list>
        </v-menu>
      </v-layout>

      <edit-station :is-new="isAddNew" :show-edit-dialog="showEditStationDialog"
          :station="curStation"
          v-on:save="onEditStationSave" v-on:cancel="onEditStationCancel"></edit-station> 
      <v-dialog v-model="showAddCommentDialog" 
        scrollable persistent 
        height="250px" max-width="85vw" style="z-index: 999">
        <v-card class="elevation-20 ">
          <v-card-title
              primary-title
            >
              增加备注
            </v-card-title>
          <v-card-text style="">            
                  <v-textarea v-model="newComment"
                    label="备注"
                  ></v-textarea>
          </v-card-text>
          <v-divider></v-divider>   
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn flat @click="addCommentCancel">取消</v-btn>
            <v-btn color="primary" flat @click="addCommentSave">保存</v-btn>
          </v-card-actions>
        </v-card>
        <v-spacer></v-spacer>
      </v-dialog>   
    </v-card>
  </v-container>
</template>

<script>
import moment from 'moment';
import allcities from '../config/cities.json';
import demostations from '../config/stations.json';
import EditStation from '../components/EditStation.vue';
import VueAMap from 'vue-amap';
import { lazyAMapApiLoaderInstance } from 'vue-amap';

let amapManager = new VueAMap.AMapManager();

export default {
  name: "Stations",
  components: {
    EditStation
  },
  data() {
    return {
      displayMode: "map",
      searchKey: "",
      city: '北京',
      filter: {},

      refreshFlag: true,
      refreshInterval: 15,
      pageSize: 30,

      filterMenu: false,
      cityDialog: false,
      stations: [], 
      curStation: null, 
      stLevels: [1, 2, 3, 4, 5],
      
      isAddNew: false,
      newComment: null,
      //amap 
      // map: null,
      map_searchCity: null,
      map_autoComplete: null,

      amapManager,
      mapZoom: 12,
      mapCenter: [116.397428, 39.90923],
      mapEvents: {
        init: (o) => {
          console.log(o.getCenter())
          console.log(this.$refs.map.$$getInstance())
          o.getCity(result => {
            console.log(result)
          })
        },
        'moveend': () => {
        },
        'zoomchange': () => {
        },
        'click': (e) => {
          if (this.map_temp_menu_show) {
            this.map_temp_menu_show = false;
          }
          if (this.map_station_menu_show) {
            this.map_station_menu_show = false;
          }
          console.log("Map clicked");
        },
        'touchstart': (e) => {
          this.map_touch_start = moment();
          if (document.getElementById('searchKey') === document.activeElement) {
            document.getElementById('searchKey').blur();
          }
        },
        'touchmove': (e) => {
          //this.map_touch_start = null;
        },
        'touchend': (e) => {        
          if (this.map_touch_start) {          
            let elapsed = moment().diff(this.map_touch_start);
            console.log(`MAP Touch End @ ${JSON.stringify(e.lnglat)}`);
            //if long touch over 2 seconds, we will need add a temp marker on the map
            if (elapsed > 2000) {
              console.log(`MAP Long touch on ${e.lnglat}`);
              this.addTempMarker(e.lnglat);
            }
            this.map_touch_start = null;
          }
        }
      },
      mapPlugins: [
        'ToolBar', {
          pName: 'MapType',
          defaultType: 0,
          events: {
            init(o) {
              console.log(o);
            }
          }
        }
      ],
      mapStyle: "amap://styles/8c300aebf1b10327aa1e687d2fe8e654",
      markers: [],
      tempMarker: //used for creating new station
      {
        position: [0, 0],
        events: {
          'click': (e) => {
            this.map_temp_menu_x = e.pixel.getX();
            this.map_temp_menu_y = e.pixel.getY();
            this.map_temp_menu_show = true;      
          }          
        },
        visible: false,
        draggable: true
      },

      searchOption: {
        city: '北京',
        citylimit: true
      },

      provinces: [],
      cities: [],
      curProvince: null,
      curCity: null,
      
      map_touch_start: null, //record the time when user start touch on map      
      map_temp_menu_ready: false, //used for checking whether should show the menu of temp marker 
      map_temp_menu_show: false,
      map_temp_menu_x: 0,
      map_temp_menu_y: 0,
      map_station_menu_show: false, //show menu of station marker
      map_station_menu_x: 0,
      map_station_menu_y: 0,

      showNewStationDialog: false,
      showEditStationDialog: false,
      showAddCommentDialog: false
    };
  },
  computed: {
    hasMoreData: {
      get() {
        return true;
      }
    },
    pageTitle: {
      get() {
        if (this.curCity) {
          this.city = this.curCity.name;
        }
        return '基站 - ' + this.city;
      }
    }
  },
  methods: {
    //diaplay mode
    changeDisplayMode(mode) {
      this.displayMode = mode;
      console.log(`display mode changed to ${this.displayMode}`);
    },

    //search city
    applyCity() {
      this.cityDialog = false;
      if (this.curCity) {
        this.mapZoom = 11;
        this.mapCenter = [this.curCity.center.lng, this.curCity.center.lat];
        if (this.curCity.citycode) {
          this.map_autoComplete.setCity(this.curCity.citycode);
          this.map_autoComplete.setCityLimit(true);
        }        
      }
    },        
    selectCity() {
      this.cityDialog = true;
    },
    changeProvince() {
      this.cities = [];
      if (this.curProvince) {
        this.cities = this.curProvince.districtList;
        this.curCity = this.cities[0];
      }
    },

    onMapSearchResult(pois) {
      let latSum = 0;
      let lngSum = 0;
      if (pois.length > 0) {
        pois.forEach(poi => {
          let {lng, lat} = poi;
          lngSum += lng;
          latSum += lat;
          this.markers.push([poi.lng, poi.lat]);
        });
        let center = {
          lng: lngSum / pois.length,
          lat: latSum / pois.length
        };
        this.mapCenter = [center.lng, center.lat];
      }
    },

    //filter menu
    filterCancelClick() {
      this.filterMenu = false;
    },
    filterApplyClick() {
      this.filterMenu = false;
    },

    //current geo location
    geoLocated(result) { 
      console.log(result);
      let map = this.amapManager.getMap();
      map.getCity((cityinfo) => {
        if (cityinfo && cityinfo.city) {
          this.curCity = cityinfo.city;
        }
      });
    },

    //handle when click the station marker on map or in the list
    showDetail(station) {
      this.$router.push({
        name: "stationDetail",
        params: { station: station }
      });
    },

    //reload stations
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

    //add new station
    addNewStation(station, lnglat) {
      let self = this;

      this.markers.push({
        position: [lnglat.lng, lnglat.lat],
        events: {
          click: (e) => {
            self.map_station_menu_x = e.pixel.getX();
            self.map_station_menu_y = e.pixel.getY();
            self.curStation = station;
            self.map_station_menu_show = true;
            console.log("Station Marker TOUCH END: " + e.pixel);
          },
          dragend: (e) => {
            console.log('---event---: dragend');
          }
        },
        label: {content: station.id, offset: [0, 35]},
        visible: true,
        draggable: false,
        template: `<span>${station.id}</span>`
      });
      this.stations.push(station);
    },
    calcDist(x1, y1, x2, y2) {
      let a = x2 - x1;
      let b = y2 - y1;
      return Math.sqrt( a*a + b*b );
    },
    addTempMarker(lnglat) {
      this.tempMarker.position = [lnglat.lng, lnglat.lat];
      this.tempMarker.visible = true;      
    },
    tempMenuSave(station) {
      this.showNewStationDialog = false;

      let lnglat = this.map_temp_marker.getPosition();
      this.tempMarker.visible = false;
      
      this.addNewStation(station, lnglat);            
    },
    tempMenuCancel() {
      this.showNewStationDialog = false;
    },
    cancelTempMarker() {
      this.tempMarker.visible = false;
    },

    //edit station
    onEditStationMenuClicked() {
      this.map_station_menu_show = false;
      this.showEditStationDialog = true;
    },
    onEditStationSave() {
      this.showEditStationDialog = false;
    },
    onEditStationCancel() {
      this.showEditStationDialog = false;
    },
    onAddNewStationClicked() {
      this.curStation = {};
      this.showEditStationDialog = true;
    },
    onViewStationMenuClicked() {
      this.map_station_menu_show = false;
      this.showDetail(this.curStation);
    },
    onAddCommentMenuClicked() {
      this.map_station_menu_show = false;
      this.showAddCommentDialog = true;
    },

    //load stations from server
    loadStations() {
      this.$utils.showLoading();
      //load some demo data
      demostations.forEach(ds => {
        this.addNewStation(ds, ds.position);
      });
      this.$utils.hideLoading();
      // this.$http
      //   .get("/api/stations/list")
      //   .then(result => {
      //     this.$utils.hideLoading();
      //     var retData = result.data;
      //     this.stations = _.orderBy(retData, [], []);
      //     this.showFirstPage();
      //   })
      //   .catch(error => {
      //     this.$utils.hideLoading();
      //     var errorMsg = "";
      //     if (
      //       error &&
      //       error.response &&
      //       error.response.status &&
      //       error.response.status === 401
      //     ) {
      //       errorMsg = "获取站点数据超时";
      //     } else {
      //       errorMsg = `获取站点数据时出错: ${error.message}`;
      //     }

      //     this.$utils.toast(errorMsg);
      //   });

    },

    //initialize map
    initMap() {    
      lazyAMapApiLoaderInstance.load().then(() => {
        let map = this.amapManager.getMap();
         //auto-complete
        let autoOptions = {
          input: 'searchKey',
          city: this.city,
          citylimit: true 
        };
        this.map_autoComplete = new AMap.Autocomplete(autoOptions);        
        this.map_autoComplete.on('select', (e) => {
          console.log(`select search result: ${JSON.stringify(e)}`);
          if (e && e.poi) {
            this.searchKey = e.poi.name;
            map.setZoomAndCenter(15, e.poi.location);
          }          
        });
        this.map_autoComplete.on('complete', (e) => {          
        });
        this.map_autoComplete.on('error', (e) => {
          console.error(`Error when select search result: ${JSON.stringify(e)}`);
        });

        //get current location
        let geolocation = new AMap.Geolocation({
          enableHighAccuracy: true,
          timeout: 30000,
          buttonOffset: new AMap.Pixel(10, 20),
          zoomToAccuracy: true,     
          buttonPosition: 'RB',
          showCircle: false,
          useNative: true
        });
        map.addControl(geolocation);
        
        geolocation.on('complete', (result) => {
          this.geoLocated(result);
        });
        geolocation.on('error', (err) => {
          console.error(err);
        });
        if (!geolocation.isSupported()) {
          console.error('Geo Location is not supported');
        }
      });
      
      //   //search cities
      //   this.map_searchCity = new AMap.DistrictSearch({
      //     level: 'country',
      //     subdistrict: 2
      //   });

      //   //load cities
      this.provinces = allcities;
        
    },

    addCommentCancel() {
      this.showAddCommentDialog = false;
      this.newComment = "";
    },
    addCommentSave() {
      this.showAddCommentDialog = false;
      this.curStation.comments.push({
        comment: this.newComment,
        user: "xiaodong"
      });
      this.newComment = "";
    }
  },
  mounted: function() {
    this.initMap();
    this.loadStations();
  },
  activated: function() {
  },
  deactivated: function() {}
};
</script>

<style scoped>
.search-box {
  position: absolute;
  top: 25px;
  left: 20px;
}
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
.map {
  width:100vw; 
  height: 78vh;
}
</style>
