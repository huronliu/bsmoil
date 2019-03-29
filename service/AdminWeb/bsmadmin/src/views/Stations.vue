<template>
  <v-container fluid>
    <v-layout class="mt-3 mx-5" column>
      <v-flex xs12>
        <v-toolbar flat class="transparent">
          <v-toolbar-title>基站管理</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-btn icon @click="refresh">
            <v-icon>refresh</v-icon>
          </v-btn>
          <v-btn icon @click="addStation">
            <v-icon>add_circle_outline</v-icon>
          </v-btn>          
        </v-toolbar>
      </v-flex>
      <v-flex xs12>
        <v-data-table
          :headers="headers"
          :items="stations"
          class="elevation-1"
        >
          <template slot="items" slot-scope="props">
            <td>{{ props.item.name }}</td>
            <td class="text-xs-right">{{ props.item.tag }}</td>
            <td class="text-xs-right">{{ props.item.city }}</td>
            <td class="text-xs-right">{{ props.item.province }}</td>
            <td class="text-xs-right">{{ props.item.disabled }}</td>
            <td class="text-xs-right">{{ tolocaltime(props.item.createdAt) }}</td>
            <td class="text-xs-right">
              <v-icon
                small
                class="mr-2"
                @click="editCoordinators(props.item)"
              >
                speaker_phone
              </v-icon>
            </td>
            <td class="text-xs-right">
              <v-icon
                small
                class="mr-2"
                @click="editComments(props.item)"
              >
                speaker_notes
              </v-icon>
            </td>
            <td class="justify-center layout px-0">
              <v-icon
                small
                class="mr-2"
                @click="editStation(props.item)"
              >
                edit
              </v-icon>
              <v-icon
                small
                @click="disableStation(props.item)"
              >
                remove_circle_outline
              </v-icon>
            </td>
          </template>
        </v-data-table>
      </v-flex>
    </v-layout>

    <!-- edit station dialog -->
    <v-dialog v-model="showEditStation" persistent max-width="450">
      <v-form ref="stationform" v-model="isValid">
        <v-card>
          <v-list dense class="px-3 pt-3">
            <v-list-tile class="mx-3 mt-4">
              <v-layout>
                <v-flex xs12>
                  <v-text-field
                    v-model="station.name"
                    :rules="[rules.required]"
                    type="text"
                    name="st_name"
                    label="基站名称"
                    class="caption"
                  ></v-text-field>
                </v-flex>
              </v-layout>
            </v-list-tile>
            <v-list-tile class="mx-3 mt-4">
              <v-layout>
                <v-flex xs12>
                  <v-text-field
                    v-model="station.tag"
                    type="text"
                    name="st_tag"
                    label="标注"
                    class="caption"
                  ></v-text-field>
                </v-flex>
              </v-layout>
            </v-list-tile>            
            <v-list-tile class="mx-3 mt-4">
              <v-layout>
                <v-flex xs12>
                  <v-select 
                    label="省"
                    v-model="station.province"
                    :items="provinces"
                    item-text="name"
                    item-value="name"
                    class="caption" @change="provincechange"></v-select>
                </v-flex>
              </v-layout>
            </v-list-tile>
            <v-list-tile class="mx-3 mt-4">
              <v-layout>
                <v-flex xs12>
                  <v-select
                    v-model="station.city"
                    :items="cities"
                    item-text="name"
                    item-value="name"
                    name="st_city"
                    label="城市"
                    class="caption"
                  ></v-select>
                </v-flex>
              </v-layout>
            </v-list-tile>
            <v-list-tile class="mx-3 mt-4">
              <v-layout>
                <v-flex xs12>
                  <v-text-field
                    v-model="station.lat"
                    type="text"
                    name="st_lat"
                    label="纬度"
                    class="caption"
                  ></v-text-field>
                </v-flex>
              </v-layout>
            </v-list-tile>
            <v-list-tile class="mx-3 mt-4">
              <v-layout>
                <v-flex xs12>
                  <v-text-field
                    v-model="station.lng"
                    type="text"
                    name="st_lng"
                    label="经度"
                    class="caption"
                  ></v-text-field>
                </v-flex>
              </v-layout>
            </v-list-tile>

          </v-list>
          <v-card-actions class="mt-3 px-3">
            <v-spacer></v-spacer>
            <v-btn color="gray darken-1" flat @click="showEditStation = false">取消</v-btn>
            <v-btn color="primary darken-1" flat @click="saveStation">保存</v-btn>
          </v-card-actions>
        </v-card>
      </v-form>
    </v-dialog>

    <!-- coordinators dialog -->
    <v-dialog v-model="showCoordinators" persistent max-width="650">
      <v-card>
        <v-toolbar color="teal" dark>
          <v-toolbar-title>{{station.name}}  的协调器</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-btn icon @click="showCoordinators = false">
            <v-icon>close</v-icon>
          </v-btn>
        </v-toolbar>

        <v-expansion-panel popout>
          <v-expansion-panel-content
            v-for="item in coordinators"
            :key="item.seqid"            
          >
            <template v-slot:header>
              <span><v-icon>{{item.icon}}</v-icon> &nbsp;
                  {{ item.coordinator.address? `${item.coordinator.address} (${item.coordinator.name})` : '未设置' }}
              </span>
            </template>
            <v-card class="mx-4">
              <v-list dense>
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
              <v-card-actions class="mt-3 px-3">
                <v-spacer></v-spacer>
                <v-btn color="primary darken-1" flat @click="saveCoordinator(item)">保存</v-btn>
              </v-card-actions>
            </v-card>            
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-card>
    </v-dialog>

    <!-- comments dialog -->
    <v-dialog v-model="showComments" persistent max-width="650">
      <v-card>
        <v-toolbar color="teal" dark>
          <v-toolbar-title>{{station.name}}  的备注</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-btn icon @click="showComments = false">
            <v-icon>close</v-icon>
          </v-btn>
        </v-toolbar>

        <v-layout wrap row class="mx-5 my-2" align-end>
          <v-flex xs11>
            <v-textarea
              name="newcomment"
              label="在这里输入新备注"
              v-model="newcomment"
            ></v-textarea>            
          </v-flex>
          <v-flex xs1>
            <v-btn small icon @click="addComment">
              <v-icon>save</v-icon>
            </v-btn>
          </v-flex>
        </v-layout>

        <v-list two-line dense>
          <template v-for="(comment, index) in comments">
            <v-divider
              :key="index"
              inset
            ></v-divider>
            <v-list-tile :key="comment.id" avatar>
              <v-list-tile-avatar>
                <v-icon>comment</v-icon>
              </v-list-tile-avatar>
              <v-list-tile-content>
                <v-list-tile-title v-html="comment.comment"></v-list-tile-title>
                <v-list-tile-sub-title>@{{tolocaltime(comment.commentAt)}}</v-list-tile-sub-title>
              </v-list-tile-content>
            </v-list-tile>
          </template>
        </v-list>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script>
import axios from 'axios';

export default {
  name: 'Stations',
  data() {
    return {
      drawer: true,
      stations: [],
      headers: [
        {text: '名称', value: 'name'} , 
        {text: '标注', value: 'tag', align: 'right'},
        {text: '城市', value: 'city', align: 'right'},
        {text: '省', value: 'province', align: 'right'},
        {text: '禁用', value: 'disabled', align: 'right'}, 
        {text: '创建时间', value: 'createdAt', align: 'right'},
        {text: '协调器', value: '', align: 'right'}, 
        {text: '备注', value: '', align: 'right'},
        {text: '操作', value: '', align: 'center'}
      ],
      showEditStation: false,
      station: {},
      isAdding: true,
      isValid: true,
      rules: {
        required: value => !!value || "必填",
        pinMin: v => v.length >= 4 || "Min 4 characters",
        pinMax: v => v.length <= 8 || "Max 8 characters",
        pinMatch: () =>
          this.veriPin === this.newPin || "Password does not match",
        firstName: [
          v => !!v || this.$t("enroll.firstName_required"),
          v => (v && v.length <= 40) || this.$t("enroll.firstName_maxLength")
        ],
        lastName: [
          v => !!v || this.$t("enroll.lastName_required"),
          v => (v && v.length <= 40) || this.$t("enroll.lastName_maxLength")
        ],
        birthdate: [v => !!v || this.$t("enroll.birthday_required")],
        email: [v => !v || (/.+@.+/.test(v) || this.$t("enroll.email_valid"))]
      },

      showCoordinators: false,
      coordinators: [
        { icon: 'looks_one', seqid: 1, coordinator: {} },
        { icon: 'looks_two', seqid: 2, coordinator: {} },
        { icon: 'looks_3', seqid:3, coordinator: {} },
        { icon: 'looks_4', seqid:4, coordinator: {} },
      ],
      showComments: false,
      comments: [],
      newcomment: null,
      provinces: [],
      cities: []
    }
  },
  methods: {
    refresh() {
      this.retrieveStations();
    },
    apiurl() {
      let setting = JSON.parse(this.$ls.get('bsm_setting'));
      return `http://${setting.host}:${setting.apiPort}/api`;
    },
    retrieveCities() {
      this.provinces = [];
      this.cities = [];
      axios.get(this.apiurl() + '/cities', {
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          "Access-Control-Allow-Origin": "*",
        }, 
        data: {}
      }).then(result => {
        this.provinces = result.data;
      }).catch(err => {
        this.$util.error(`获取城市列表出错: ${err.message}`);
      });
    },
    retrieveStations() {
      this.stations = [];
      this.$util.showLoading();
      axios.get(this.apiurl() + '/stations', {
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          "Access-Control-Allow-Origin": "*",
        }, 
        data: {}
      })
      .then(result => {
        this.$util.hideLoading();
        this.stations = result.data;
      }).catch(err => {
        this.$util.hideLoading();
        this.$util.error(`${err.response.data}`);
      });
    },
    retrieveCoordinators(st) {
      if (st) {
        for (let i = 0; i < 4; i++) {
          this.coordinators[i].coordinator = {};
        }
        this.$util.showLoading();
        axios.get(this.apiurl() + '/stations/' + st.id + '/coordinators', {
          headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          "Access-Control-Allow-Origin": "*",
          }, 
          data: {}
        }).then(result => {
          this.$util.hideLoading();
          let data = result.data;
          if (data && data.length > 0) {
            data.forEach(co => {
              if (co.seqId >= 1 && co.seqId <= 4) {
                this.coordinators[co.seqId-1].coordinator = co;
              }
            });
          }
        }).catch(err => {
          this.$util.hideLoading();
          this.$util.error(`${err.response.data}`);
        });
      }
    },
    retrieveComments(st) {
      if (st) {
        this.comments = [];
        this.$util.showLoading();
        axios.get(this.apiurl() + '/stations/' + st.id + '/comments', {
          headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          "Access-Control-Allow-Origin": "*"
          }, 
          data: {}
        }).then(result => {
          this.$util.hideLoading();
          this.comments = result.data;
        }).catch(err => {
          this.$util.hideLoading();
          this.$util.error(`${err.response.data}`);
        }); 
      }
    },
    addStation() {
      this.station = {};
      this.isAdding = true;
      this.isValid = true;
      this.showEditStation = true;
    },
    editStation(st) {
      this.station = st;
      this.isAdding = false;
      this.isValid = true;
      this.showEditStation = true;
    },
    saveStation() {
      if (this.$refs.stationform.validate()) {
        if (this.isAdding) {
          this.$util.showLoading();
          axios.post(this.apiurl() + '/stations', this.station)
          .then(() => {
            this.$util.hideLoading();
            this.showEditStation = false;
            this.$util.info('添加基站信息成功');
            this.retrieveStations();
          }).catch(err => {
            this.$util.hideLoading();
            this.$util.error(`${err.response.data}`);
          })
        } else {
          this.$util.showLoading();
          axios.patch(this.apiurl() + '/stations/' + this.station.id, this.station)
          .then(() => {
            this.$util.hideLoading();
            this.showEditStation = false;
            this.$util.info('更新基站信息成功');
            this.retrieveStations();
          }).catch(err => {
            this.$util.hideLoading();
            this.$util.error(`${err.response.data}`);
          })
        }
      }
    },
    editComments(st) {
      this.station = st;
      this.showComments = true;
      this.newcomment = null;
      this.retrieveComments(st);
    },
    addComment() {
      if (this.newcomment) {
        this.$util.showLoading();
        axios.post(`${this.apiurl()}/stations/${this.station.id}/comments`, {
          comment: this.newcomment
        })
        .then(() => {
          this.$util.hideLoading();
          this.$util.info('添加备注成功');
          this.newcomment = null;
          this.retrieveComments(this.station);
        }).catch(err => {
          this.$util.hideLoading();
          this.$util.error(`${err.response.data.Message}`);
        });
      }
    },
    editCoordinators(st) {
      this.station = st;
      this.showCoordinators = true;
      this.retrieveCoordinators(st);
    },
    saveCoordinator(coor) {
      if (coor) {
        this.$util.showLoading();
        axios.post(`${this.apiurl()}/stations/${this.station.id}/coordinators/${coor.seqid}`, coor.coordinator)
        .then(() => {
          this.$util.hideLoading();
          this.$util.info('添加协调器信息成功');
          this.retrieveCoordinators(this.station);
        }).catch(err => {
          this.$util.hideLoading();
          this.$util.error(`${err.response.data}`);
        });
      }
    },
    tolocaltime(time) {
      let dt = new Date(time);
      return dt.toLocaleString('en-US', { timezone: 'Asia/Shanghai'});
    },
    provincechange(item) {
      this.cities = [];
      this.provinces.forEach((pro) => {
        if (pro.name === item) {
          this.cities = pro.districtList;
        }
      });
    }
  },
  beforeMount() {
    this.retrieveCities();
  }
}
</script>

