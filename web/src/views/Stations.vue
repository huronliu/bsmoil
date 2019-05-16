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
          <v-layout v-show="this.displayStations.length === 0" justify-center>
            <p class="headline mt-5">没有数据</p>
          </v-layout>
        </v-flex>
        <v-flex xs12>
            <v-list two-line dense>
              <template v-for="(st, index) in displayStations">
                <v-list-tile                  
                  :key="st.id"
                  avatar
                  @click.native="showDetail(st)"
                  class="stationItem"
                >
                  <v-list-tile-avatar>
                      <v-icon small>fas fa-broadcast-tower</v-icon>
                  </v-list-tile-avatar>
                  
                  <v-list-tile-content>
                    <v-list-tile-title class="text--primary">                      
                      {{ st.name }} - [{{st.tag}}]
                    </v-list-tile-title>
                    <v-list-tile-sub-title>
                      <v-icon class="title_icon" small>place</v-icon>
                      <span class="disable_pn">{{st.city}}, {{st.province}}</span>
                    </v-list-tile-sub-title>
                  </v-list-tile-content>

                  <v-list-tile-action>
                    <v-btn icon ripple>
                      <v-icon small>navigate_next</v-icon>
                    </v-btn>
                  </v-list-tile-action>                     
                </v-list-tile>
                <v-divider v-if="index + 1 < stations.length" :key="`divider-${index}`" class="mx-2"></v-divider>
              </template>  
            </v-list>
        </v-flex>
        <v-flex xs12>
          <v-layout v-if="hasMoreData" justify-center>
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
          <v-list v-if="!isMovingStation">
            <v-list-tile>
              <v-list-tile-title @click="onViewStationMenuClicked">查看基站数据</v-list-tile-title>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-title @click="onEditStationMenuClicked">编辑基站信息</v-list-tile-title>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-title @click="onMoveStationMenuClicked">移动基站位置</v-list-tile-title>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-title @click="onAddCommentMenuClicked">增加备注</v-list-tile-title>
            </v-list-tile>
          </v-list>
          <v-list v-if="isMovingStation">
            <v-list-tile>
              <v-list-tile-title @click="onMoveEndMenuClicked">移动结束</v-list-tile-title>
            </v-list-tile>
          </v-list>
        </v-menu>
      </v-layout>

      <v-btn absolute icon class="addbtn" color="primary" @click="createStation">
        <v-icon>add</v-icon>
      </v-btn>

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
import EditStation from '../components/EditStation.vue';
import VueAMap from 'vue-amap';
import { lazyAMapApiLoaderInstance } from 'vue-amap';
import api from '../modules/api.js';

let amapManager = new VueAMap.AMapManager();

export default {
  name: "Stations",
  props: ['displayStationId'],
  components: {
    EditStation
  },
  data() {
    return {
      displayMode: "list", //map or list
      searchKey: "",
      city: '北京',
      filter: {},

      stations: [], 
      displayStations: [],
      curStation: {}, 

      pageSize: 30,

      filterMenu: false,
      cityDialog: false,
      
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
          console.log(`AMap initialized: ${o.getCenter()}`);
          o.getCity(result => {
            console.log(`Current city is ${JSON.stringify(result)}`);
          })
        },
        'moveend': () => {
        },
        'zoomchange': () => {
        },
        'click': () => {
          if (this.map_temp_menu_show) {
            this.map_temp_menu_show = false;
          }
          if (this.map_station_menu_show) {
            this.map_station_menu_show = false;
          }
        },
        'touchstart': () => {
          this.map_touch_start = moment();
          if (document.getElementById('searchKey') === document.activeElement) {
            document.getElementById('searchKey').blur();
          }
        },
        'touchmove': () => {
          this.map_touch_start = null;
        },
        'touchend': (e) => {        
          if (this.map_touch_start) {          
            let elapsed = moment().diff(this.map_touch_start);
            this.map_touch_start = null;
            console.log(`MAP Touch End @ ${JSON.stringify(e.lnglat)}`);
            //if long touch over 2 seconds, we will need add a temp marker on the map
            if (elapsed > 2000) {
              this.addTempMarker(e.lnglat);
            }            
          }
        }
      },
      mapPlugins: [],
      mapStyle: "amap://styles/8c300aebf1b10327aa1e687d2fe8e654",

      markers: [],
      curMarker: null,
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
      map_temp_menu_show: false, //whether show the temp maker menu
      map_temp_menu_x: 0, //x position of the temp marker menu
      map_temp_menu_y: 0, //y position of the temp marker menu
      map_station_menu_show: false, //whether show menu of station marker
      map_station_menu_x: 0, //x position of the station menu
      map_station_menu_y: 0, //y position of the station menu

      showNewStationDialog: false, //whether show the New Station dialog
      showEditStationDialog: false,//Whether show the Edit station dialog
      showAddCommentDialog: false,  //Whether show the Add Comment dialog

      isMovingStation: false //whether the moving station menu clicked
    };
  },
  computed: {
    hasMoreData: {
      get() {
        return this.stations.length > this.displayStations.length;
      }
    },
    pageTitle: {
      get() {
        if (this.curCity) {
          this.city = this.curCity.name;
        }
        return `基站 - [${this.city}]`;
      }
    }
  },
  methods: {
    //diaplay mode
    changeDisplayMode(mode) {
      this.displayMode = mode;
      console.log(`display mode changed to ${this.displayMode}`);
    },

    //select another city
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
    selectCity() { //show the select city dialog
      this.cityDialog = true;
    },
    changeProvince() { //select another province
      this.cities = [];
      if (this.curProvince) {
        this.cities = this.curProvince.districtList;
        this.curCity = this.cities[0];
      }
    },
  

    //filter menu
    filterCancelClick() {
      this.filterMenu = false;
    },
    filterApplyClick() {
      this.filterMenu = false;
    },

    //navigate to the current geo location
    geoLocated(result) { 
      console.log(result);
      let map = this.amapManager.getMap();
      map.getCity((cityinfo) => {
        if (cityinfo && cityinfo.city) {
          this.curCity = cityinfo.city;
        }
      });
    },

    //reload stations ----------------------
    refresh() {
      this.loadStations();
      window.scrollTo(0, 0);
    },
    showFirstPage() {
      if (this.stations.length <= this.pageSize) {
        this.displayStations = this.stations;
      } else {
        this.displayStations = this.stations.slice(0, this.pageSize);
      }
    },
    showMore() {
      if (this.hasMoreData) {
        this.displayStations.push(this.stations.slice(this.displayStations.length, this.pageSize));
      } else {
        this.$utils.toast("没有更多");
      }      
    },
    //---------------------------------------

    //add new station on the map
    // addNewStation(station, lnglat) {
    //   let self = this;

    //   this.markers.push({
    //     position: [lnglat.lng, lnglat.lat],
    //     events: {
    //       click: (e) => {
    //         self.map_station_menu_x = e.pixel.getX();
    //         self.map_station_menu_y = e.pixel.getY();
    //         self.curStation = station;
    //         self.map_station_menu_show = true;
    //       },
    //       dragend: () => {
    //         console.log('---event---: dragend');
    //       }
    //     },
    //     label: {content: station.id, offset: [0, 35]},
    //     visible: true,
    //     draggable: false,
    //     template: `<span>${station.id}</span>`
    //   });
    //   this.stations.push(station);
    // },

    //show the existing stations on the map
    addStationOnMap(station) {
      let self = this;
      if (station && station.lat && station.lng) {
        this.markers.push({
          position: [station.lng, station.lat],
          offset: [-13,30],
          events: {
            click: (e) => {
              self.map_station_menu_x = e.pixel.getX();
              self.map_station_menu_y = e.pixel.getY();
              self.curStation = station;
              self.map_station_menu_show = true;
            },
            dragend: (e) => {
              console.log(`event--dragend: ${e.lnglat}`);
              self.curStation.lng = e.lnglat.lng;
              self.curStation.lat = e.lnglat.lat;
              api.saveStation(self.curStation.id, self.curStation)
              .then(result => {
                this.$utils.toast(`基站新位置已保存`);
              }).catch(err => {
                this.$utils.toast(`保存位置出错: ${err.message}`);
              });
            }
          },
          label: {content: station.name, offset: [0, 35]},
          visible: true,
          draggable: false,
          template: `<span>${station.name}</span>`,
          stationId: station.id
        });
      }
    },

    calcDist(x1, y1, x2, y2) {
      let a = x2 - x1;
      let b = y2 - y1;
      return Math.sqrt( a*a + b*b );
    },

    //Add or move the temp marker on map
    addTempMarker(lnglat) {
      this.tempMarker.position = [lnglat.lng, lnglat.lat];
      this.tempMarker.visible = true;      
    },

    //Add new station --------------------
    onAddNewStationClicked() {
      this.curStation = {};
      this.curStation.lng = this.tempMarker.position[0];
      this.curStation.lat = this.tempMarker.position[1];      
      this.isAddNew = true;
      console.log(`new station loc: ${this.curStation.lng}, ${this.curStation.lat}`);
      this.showEditStationDialog = true;
    },

    createStation() {
      this.curStation = {};
      if (this.displayMode === 'map') {
        let center = this.$refs.map.$$getCenter();
        this.curStation.lng = center[0];
        this.curStation.lat = center[1];
      }      
      this.isAddNew = true;
      console.log(`new station loc: ${this.curStation.lng}, ${this.curStation.lat}`);
      this.showEditStationDialog = true;
    },

    //when click the Save on the Add New Station dialog
    // tempMenuSave(station) {
    //   this.showNewStationDialog = false;
    //   let lnglat = this.map_temp_marker.getPosition();
    //   this.tempMarker.visible = false;      
    //   this.addNewStation(station, lnglat);            
    // },
    // tempMenuCancel() {
    //   this.showNewStationDialog = false;
    // },
    cancelTempMarker() {
      this.tempMarker.visible = false;
    },
    //-----------------------------------------------    

    //edit station ----------------------
    onEditStationMenuClicked() {
      this.map_station_menu_show = false;
      this.showEditStationDialog = true;
    },    
    onEditStationSave() {
      this.showEditStationDialog = false;
      this.$utils.toast(`基站信息已保存`);
      this.loadStations();
    },
    onEditStationCancel() {
      this.showEditStationDialog = false;
    },
    //-----------------------------------

    //Move station ----------------------
    onMoveStationMenuClicked() {
      this.map_station_menu_show = false;
      this.curMarker = null;
      if (this.curStation) {
        this.isMovingStation = true;
        for (let i = 0; i < this.markers.length; i++) {
          let marker = this.markers[i];
          if (marker.stationId === this.curStation.id) {
            marker.draggable = true;    
            this.curMarker = marker;        
            return;
          }
        };
      }      
    },
    onMoveEndMenuClicked() {
      this.map_station_menu_show = false;
      this.isMovingStation = false;      
      if (this.curMarker) {
        this.curMarker.draggable = false;        
      }
    },
    //-----------------------------------

    //click the View Station data on menu
    //-----------------------------------
    onViewStationMenuClicked() {
      this.map_station_menu_show = false;
      this.showDetail(this.curStation);
    },
    showDetail(station) {
      this.$router.push({
        name: "stationDetail",
        params: { station: station }
      });
    },
    //----------------------------------

    //click the Add Comment on menu
    //------------------------------
    onAddCommentMenuClicked() {
      this.map_station_menu_show = false;
      this.showAddCommentDialog = true;
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
    },
    // -----------------------------

    //load stations from server
    loadStations() {
      this.$utils.showLoading();
      
      this.stations = [];
      this.markers = [];
      api.getStations()
        .then(result => {
          this.$utils.hideLoading();
          if (result && result.length > 0) {
            var retData = result;
            this.stations = _.orderBy(retData, [], []);
            this.showFirstPage();
            this.stations.forEach(st => {
              this.addStationOnMap(st);
            });
          }          
        })
        .catch(err => {
          this.$utils.hideLoading();          
          this.$utils.toast(`获取基站列表失败: ${err.message}`);
        });
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
        this.map_autoComplete.on('complete', () => {          
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
          this.$utils.toast('不支持获取地理位置');
        }

        this.map = map;
      });
      
      //   //search cities
      //   this.map_searchCity = new AMap.DistrictSearch({
      //     level: 'country',
      //     subdistrict: 2
      //   });

    },   
    initCities() {
      api.getCitiesList().then(result => {
        this.provinces = result;
      }).catch(err => {
        this.$$utils.toast(`获取城市列表出错: ${err.message}`);
      });
    } 
  },
  beforeMount() {
    this.initCities();
  },
  mounted() {    
    this.initMap();
    this.loadStations();
  },
  activated() {
    if (this.displayStationId && this.displayStationId > 0) {
      console.log(`Stations page is activated with station id: ${this.displayStationId}`);
      this.displayMode = 'map';
      this.markers.forEach(mk => {
        if (mk.stationId === this.displayStationId) {
          this.map.setZoomAndCenter(15, mk.position);          
        }
      });
      //this.displayStationId = 0;
    } else {
      console.log(`Stations page is activated without station id specified`);
    }
  }
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
.addbtn {
  display: block;
  position: fixed; /* Fixed/sticky position */
  bottom: 80px; /* Place the button at the bottom of the page */
  left: 20px; /* Place the button 30px from the right */
  z-index: 99; /* Make sure it does not overlap */
  border: none; /* Remove borders */
}
</style>
