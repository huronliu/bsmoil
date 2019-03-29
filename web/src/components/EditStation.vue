<template>
  <v-container fluid>
    <v-dialog v-model="showEditStation" scrollable persistent max-width="85vw" style="z-index: 899">
      <v-card class="elevation-20 ">
        <v-card-title
            primary-title
          >
            {{title}}
            <v-spacer></v-spacer>
            <v-btn small color="primary" @click="editCoordinator">编辑协调器</v-btn>
          </v-card-title>
        <v-card-text style="">        
          <v-list two-line subheader dense>
            <v-list-tile class="filter_toolbar_item">
              <v-list-tile-action>
                <v-text-field v-model="station.name"
                  label="基站名称"
                ></v-text-field>
              </v-list-tile-action>
            </v-list-tile>
          </v-list>
          <v-list two-line subheader dense>
            <v-list-tile class="filter_toolbar_item">
              <v-list-tile-action>
                <v-text-field v-model="station.tag"
                  label="基站标注"
                ></v-text-field>
              </v-list-tile-action>
            </v-list-tile>
          </v-list>
          <v-list two-line subheader dense>
            <v-list-tile class="filter_toolbar_item">
              <v-list-tile-action>
                <v-select 
                  v-model="station.province"
                  label="省"
                  :items="es_provinces"
                  item-text="name"
                  item-value="name"
                  @change="provincechanged"
                ></v-select>
              </v-list-tile-action>
            </v-list-tile>
          </v-list>
          <v-list two-line subheader dense>
            <v-list-tile class="filter_toolbar_item">
              <v-list-tile-action>
                <v-select 
                  v-model="station.city"
                  label="城市"
                  :items="es_cities"
                  item-text="name"
                  item-value="name"
                ></v-select>
              </v-list-tile-action>
            </v-list-tile>
            <v-list-tile class="filter_toolbar_item">
                <v-list-tile-action>
                    <v-text-field
                      v-model="station.lat"
                      type="text"
                      name="st_lat"
                      label="纬度"
                    ></v-text-field>
                </v-list-tile-action>
              </v-list-tile>
              <v-list-tile class="filter_toolbar_item">
                <v-list-tile-action>
                    <v-text-field
                      v-model="station.lng"
                      type="text"
                      name="st_lng"
                      label="经度"
                    ></v-text-field>
                </v-list-tile-action>
              </v-list-tile>
          </v-list>
        </v-card-text>
        <v-divider></v-divider>   
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn flat @click="cancel">取消</v-btn>
          <v-btn color="primary" flat @click="save">确定</v-btn>
        </v-card-actions>
      </v-card>
      <v-spacer></v-spacer>
    </v-dialog>

    <v-dialog v-model="showEditCoordinator"  persistent max-width="85vw" style="z-index: 999">
      <v-card>
        <v-toolbar color="teal" dark>
          <v-toolbar-title>{{station.name}}  的协调器</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-btn icon @click="showEditCoordinator = false">
            <v-icon>close</v-icon>
          </v-btn>
        </v-toolbar>

        <template v-for="item in coordinators">
          <v-card v-bind:key="item.seqid">
            <v-card-title>
              <span><v-icon>{{item.icon}}</v-icon> &nbsp;
                  {{ item.coordinator.address? `${item.coordinator.address} (${item.coordinator.name})` : '未设置' }}
              </span>
              <v-spacer></v-spacer>
              <v-btn small icon @click="item.expanded = !item.expanded"><v-icon>expand_more</v-icon></v-btn>
            </v-card-title>
            <v-divider></v-divider>
            <v-list v-if="item.expanded" dense>
              <v-list-tile class="mx-3 mt-2">
                <v-layout>
                    <v-flex xs12>
                      <v-text-field
                        v-model="item.coordinator.address"
                        type="text"
                        name="co_address"
                        label="地址"
                        class="caption"
                        :rules="[rules.required]"
                      ></v-text-field>
                    </v-flex>
                  </v-layout>
              </v-list-tile>
              <v-list-tile class="mx-3 mt-2">
                <v-layout>
                  <v-flex xs12>
                    <v-text-field
                      v-model="item.coordinator.name"
                      type="text"
                      name="co_name"
                      label="名称"
                      class="caption"
                    ></v-text-field>
                  </v-flex>
                </v-layout>                 
              </v-list-tile>
            </v-list>

            <v-card-actions class="mt-3 px-3" v-if="item.expanded">
              <v-spacer></v-spacer>
              <v-btn color="primary darken-1" flat @click="saveCoordinator(item)">保存</v-btn>
            </v-card-actions>
          </v-card>
        </template>

        
      </v-card>
    </v-dialog>
  </v-container>
  
</template>

<script>
import api from '../modules/api.js';

export default {
  name: 'EditStation',
  props: {
    showEditDialog: {
      type: Boolean, default: false, required: true
    },
    isNew: {
      type: Boolean, default: false, required: false
    },
    station: { }    
  },
  data() {
    return {
      es_provinces: [],
      es_cities: [],
      showEditCoordinator: false,
      showEditStation: false,
      coordinators: [
        { icon: 'looks_one', expanded: false, seqid: 1, coordinator: {} },
        { icon: 'looks_two', expanded: false, seqid: 2, coordinator: {} },
        { icon: 'looks_3', expanded: false, seqid:3, coordinator: {} },
        { icon: 'looks_4', expanded: false, seqid:4, coordinator: {} }
      ],
      rules: {
        required: value => !!value || "必填"
      }
    }
  },
  computed: {
    title: {
      get() {
        return this.isNew? "新增基站" : "编辑基站";        
      }
    }
  },
  watch: {
    showEditDialog(val) {
      this.showEditStation = val;
      if (val) {
        this.provincechanged(this.station.province);
      }
    },
    showEditCoordinator(val) {
      if (val) {
        this.retrieveCoordinators();
      }
    }
  },
  methods: { 
    save() {
      if (this.isNew) {
        this.$utils.showLoading();
        api.addStation(this.station).then(() => {
          this.$utils.hideLoading();
          this.showEditStation = false;
          this.$emit('save', this.station);
        }).catch(err => {
          this.$utils.hideLoading();
          this.$utils.toast(`保存基站信息出错: ${err.message}`);
        });
      } else {
        this.$utils.showLoading();
        api.saveStation(this.station.id, this.station).then(() => {
          this.$utils.hideLoading();
          this.showEditStation = false;
          this.$emit('save', this.station);
        }).catch(err => {
          this.$utils.hideLoading();
          this.$utils.toast(`保存基站信息出错: ${err.message}`);
        });
      }      
    },
    cancel() {
      this.showEditStation = false;
      this.$emit('cancel');
    },
    initCities() {
      api.getCitiesList().then(result => {
        this.es_provinces = result;
      }).catch(err => {
        this.$utils.toast(`获取城市列表出错: ${err.message}`);
      })
    },
    provincechanged(item) {
      console.log(`current province selected: ${item}`);
      this.es_cities = [];
      this.es_provinces.forEach((pro) => {
        if (pro.name === item) {
          this.es_cities = pro.districtList;
        }
      });
    },
    editCoordinator() {
      this.showEditCoordinator = true;
    },
    retrieveCoordinators() {
      for (let i = 0; i < 4; i++) {
          this.coordinators[i].coordinator = {};
          this.coordinators[i].expanded = false;
        }

      if (this.station) {        
        this.$utils.showLoading();
        api.loadCoordinators(this.station)
        .then(result => {
          this.$utils.hideLoading();
          if (result && result.length > 0) {
            result.forEach(co => {
              if (co.seqId >= 1 && co.seqId <= 4) {
                this.coordinators[co.seqId-1].coordinator = co;
              }
            });
          }
        }).catch(err => {
          this.$utils.hideLoading();
          this.$utils.toast(`${err.response.data}`);
        });
      }
    }, 
    saveCoordinator(coor) {
      if (coor) {
        this.$utils.showLoading();
        api.saveCoordinator(this.station.id, coor.seqid, coor.coordinator)
        .then(() => {
          this.$utils.hideLoading();
          this.$utils.toast('添加协调器信息成功');
          this.retrieveCoordinators();
        }).catch(err => {
          this.$utils.hideLoading();
          this.$utils.toast(`${err.response.data}`);
        });
      }
    }
  },
  activated: function() {
  },
  beforeMount: function() {
    this.initCities();
  }
}
</script>

<style>
.filter_toolbar_item {
  
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
