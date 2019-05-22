<template>
  <v-container fluid>
    <mobile-header title="基站数据">
      <template slot="left">
        <v-btn icon large v-on:click="goBack()">
          <v-icon medium>keyboard_arrow_left</v-icon>
        </v-btn>        
      </template>
      <template>
        <v-btn icon large @click="refresh()">
          <v-icon>refresh</v-icon>
        </v-btn>
      </template>
    </mobile-header>

    <!-- station info  -->
    <v-card  class="mb-1 py-2 pb-4">
      <v-layout align-center >
        <v-flex xs10 class="px-3 py-1">          
                <span class="">
                  <v-icon small>perm_device_information</v-icon>
                  {{ station.name }} [{{ station.tag }}]
                </span>
        </v-flex>
        <v-flex xs2 class="pr-5 py-2">
          <v-btn icon small @click="editstation">
            <!-- <v-icon small>edit</v-icon> -->
            <v-badge>
              <v-img :src="edit_icon" width="20px" height="20px"></v-img>
            </v-badge>
          </v-btn>        
        </v-flex>        
      </v-layout>
      <v-layout align-center justify-end row class="px-3">
        <v-flex xs12>
          <span class="">
            <v-icon small>place</v-icon>
              {{station.city}}, {{station.province}}
          </span>
        </v-flex>
      </v-layout>
    </v-card>

    <v-tabs
        slot="extension"
        v-model="seltab"
        fixed-tabs
        color=""
        v-if="!error"
        @change="tabchange"
      >
      <v-tabs-slider></v-tabs-slider>
      <v-tab href="#basic" class="primary--text">
        <!-- <v-icon>description</v-icon> -->
        <v-badge>
          <v-img :src="data_icon" width="20px" height="20px"></v-img>
        </v-badge>
      </v-tab>

      <v-tab href="#history" class="primary--text">
        <!-- <v-icon>list_alt</v-icon> -->
        <v-badge>
          <v-img :src="history_icon" width="20px" height="20px"></v-img>
        </v-badge>
      </v-tab>

      <v-tab href="#chart" class="primary--text">
        <!-- <v-icon>insert_chart_outlined</v-icon> -->
        <v-badge>
          <v-img :src="chart_icon" width="20px" height="20px"></v-img>
        </v-badge>
      </v-tab>

      <v-tab href="#comments" class="primary--text">
        <!-- <v-icon>comment</v-icon> -->
        <v-badge>
          <v-img :src="comment_icon" width="20px" height="20px"></v-img>
        </v-badge>
      </v-tab>
    </v-tabs>
   
    <v-tabs-items v-model="seltab" class="white elevation-1" v-if="!error">
      <v-tab-item id="basic">
        
        <!-- Coordinators -->
        <template v-for="(coor, index) in coordinators">
          <v-card v-if="!error" class="mb-2 pt-2" v-bind:key="index">
            <v-layout align-center>
              <v-flex>
                <v-list dense class="pl-3 pr-3">
                  <v-subheader>协调器{{coor.seqId}} - {{coor.address}}</v-subheader>
                  <v-list-tile>
                    <v-list-tile-content>
                      <v-list-tile-title>水平倾斜角度</v-list-tile-title>
                      <v-list-tile-sub-title class="caption">
                        X轴: {{toNumValue(coor.data.tilt1X)}} 度
                        Y轴: {{toNumValue(coor.data.tilt1Y)}} 度
                      </v-list-tile-sub-title>
                    </v-list-tile-content>
                  </v-list-tile>
                  <v-divider></v-divider>
                  <v-list-tile>
                    <v-list-tile-content>
                      <v-list-tile-title>垂直偏移距离</v-list-tile-title>
                      <v-list-tile-sub-title class="caption">
                        X轴: {{toNumValue(coor.data.skewingX)}} 毫米
                        Y轴: {{toNumValue(coor.data.skewingY)}} 毫米 
                      </v-list-tile-sub-title>
                    </v-list-tile-content>
                  </v-list-tile>
                  <v-divider></v-divider>
                  <v-list-tile>
                    <v-list-tile-content>
                      <v-list-tile-title>瞬时风速</v-list-tile-title>
                      <v-list-tile-sub-title class="caption">
                        {{toNumValue(coor.data.speed)}} 米/秒
                      </v-list-tile-sub-title>
                    </v-list-tile-content>
                  </v-list-tile>
                  <v-divider></v-divider>
                  <v-list-tile>
                    <v-list-tile-content>
                      <v-list-tile-title>环境温度</v-list-tile-title>
                      <v-list-tile-sub-title class="caption">
                        {{toNumValue(coor.data.temperature)}} 度
                      </v-list-tile-sub-title>
                    </v-list-tile-content>
                  </v-list-tile>
                  <v-divider></v-divider>
                  <!-- <v-list-tile>
                    <v-list-tile-content>
                      <v-list-tile-title>倾角测量仪2 {{coor.data? coor.data.tilt2_Addr : ''}}</v-list-tile-title>
                      <v-list-tile-sub-title class="caption">
                        X轴: {{coor.data? `${coor.data.tilt2_X_Positive > 0? '+' : '-'}${coor.data.tilt2_X_Degree}°${coor.data.tilt2_X_Minute}'${coor.data.tilt2_X_Second}"` : ''}} 
                        Y轴: {{coor.data? `${coor.data.tilt2_Y_Positive > 0? '+' : '-'}${coor.data.tilt2_Y_Degree}°${coor.data.tilt2_Y_Minute}'${coor.data.tilt2_Y_Second}"` : ''}} 
                      </v-list-tile-sub-title>
                    </v-list-tile-content>
                  </v-list-tile>
                  <v-divider></v-divider>
                  <v-list-tile>
                    <v-list-tile-content>
                      <v-list-tile-title>倾角测量仪3 {{coor.data? coor.data.tilt3_Addr : ''}}</v-list-tile-title>
                      <v-list-tile-sub-title class="caption">
                        X轴: {{coor.data? `${coor.data.tilt3_X_Positive > 0? '+' : '-'}${coor.data.tilt3_X_Degree}°${coor.data.tilt3_X_Minute}'${coor.data.tilt3_X_Second}"` : ''}} 
                        Y轴: {{coor.data? `${coor.data.tilt3_Y_Positive > 0? '+' : '-'}${coor.data.tilt3_Y_Degree}°${coor.data.tilt3_Y_Minute}'${coor.data.tilt3_Y_Second}"` : ''}} 
                      </v-list-tile-sub-title>
                    </v-list-tile-content>
                  </v-list-tile>
                  <v-divider></v-divider>
                  <v-list-tile>
                    <v-list-tile-content>
                      <v-list-tile-title>倾角测量仪4 {{coor.data? coor.data.tilt4_Addr : ''}}</v-list-tile-title>
                      <v-list-tile-sub-title class="caption">
                        X轴: {{coor.data? `${coor.data.tilt4_X_Positive > 0? '+' : '-'}${coor.data.tilt4_X_Degree}°${coor.data.tilt4_X_Minute}'${coor.data.tilt4_X_Second}"` : ''}} 
                        Y轴: {{coor.data? `${coor.data.tilt4_Y_Positive > 0? '+' : '-'}${coor.data.tilt4_Y_Degree}°${coor.data.tilt4_Y_Minute}'${coor.data.tilt4_Y_Second}"` : ''}} 
                      </v-list-tile-sub-title>
                    </v-list-tile-content>
                  </v-list-tile>
                  <v-divider></v-divider> -->
                  <v-list-tile>
                    <v-list-tile-content>最后数据时间</v-list-tile-content>
                    <v-list-tile-content class="caption">
                      {{coor.data? `${coor.data.receivedAt}` : ''}}
                    </v-list-tile-content>
                  </v-list-tile>  
                </v-list>
              </v-flex>
            </v-layout>
          </v-card>
        </template>
        
        
        <!-- action buttons -->
        <v-card v-if="!error" class="mb-2">
          <v-layout align-center justify-center>
            <v-flex>
              <div class="text-xs-center">
                <v-btn color="#4A87D3" dark @click.native="gotoMap">
                  查看地图
                  <v-icon right dark>map</v-icon>
                </v-btn>
                <v-btn color="#4A87D3" dark @click.native="gotoWarns">
                  查看报警记录
                  <v-icon right dark>warning</v-icon>
                </v-btn>          
              </div>
            </v-flex>
          </v-layout>
          </v-card>
      </v-tab-item>
      <v-tab-item id="history">
        <v-layout row class="pt-3">
          <v-flex xs6 class="px-2">
            <v-menu
              v-model="datemenu"
              :close-on-content-click="false"
              :nudge-right="40"
              lazy
              transition="scale-transition"
              offset-y
              full-width
            >
              <template>
                <v-text-field
                  v-model="seldate"
                  label="请选择日期"
                  prepend-icon="event"
                  readonly
                  slot="activator"
                ></v-text-field>
              </template>
              <v-date-picker v-model="seldate" @input="datechanged"></v-date-picker>
            </v-menu>
          </v-flex>
          <v-flex xs6 class="px-2">
            <v-select
              :items="coordinators"
              :item-text="coorlabel"
              return-object
              label="选择协调器"
              v-model="selcoor"
              @change="coorchanged"
            ></v-select>
          </v-flex>
        </v-layout>

        <template v-for="(data, index) in historyData">
          <v-card class="my-2"  v-bind:key="index">          
            <v-list dense class="px-3 py-2">
              <v-subheader>{{data.receivedAt}}</v-subheader>
              
              <v-list-tile>
                <v-list-tile-content>
                  <v-list-tile-title>水平倾斜角度</v-list-tile-title>
                  <v-list-tile-sub-title class="caption">
                    X轴: {{toNumValue(data.tilt1X)}} 度
                    Y轴: {{toNumValue(data.tilt1Y)}} 度
                  </v-list-tile-sub-title>
                </v-list-tile-content>
              </v-list-tile>
              <v-divider></v-divider>
              <v-list-tile>
                <v-list-tile-content>
                  <v-list-tile-title>垂直偏移距离</v-list-tile-title>
                  <v-list-tile-sub-title class="caption">
                    X轴: {{toNumValue(data.skewingX)}} 毫米
                    Y轴: {{toNumValue(data.skewingY)}} 毫米 
                  </v-list-tile-sub-title>
                </v-list-tile-content>
              </v-list-tile>
              <v-divider></v-divider>
              <v-list-tile>
                <v-list-tile-content>
                  <v-list-tile-title>瞬时风速</v-list-tile-title>
                  <v-list-tile-sub-title class="caption">
                    {{toNumValue(data.speed)}} 米/秒
                  </v-list-tile-sub-title>
                </v-list-tile-content>
              </v-list-tile>
              <v-divider></v-divider>
              <v-list-tile>
                <v-list-tile-content>
                  <v-list-tile-title>环境温度</v-list-tile-title>
                  <v-list-tile-sub-title class="caption">
                    {{toNumValue(data.temperature)}} 度
                  </v-list-tile-sub-title>
                </v-list-tile-content>
              </v-list-tile>
              <v-divider></v-divider>
              <!-- <v-list-tile>
                <v-list-tile-content>
                  <v-list-tile-title>倾角测量仪2 {{data.tilt2_Addr}}</v-list-tile-title>
                  <v-list-tile-sub-title class="caption">
                    X轴: {{`${data.tilt2_X_Positive > 0? '+' : '-'}${data.tilt2_X_Degree}°${data.tilt2_X_Minute}'${data.tilt2_X_Second}"`}} 
                    Y轴: {{`${data.tilt2_Y_Positive > 0? '+' : '-'}${data.tilt2_Y_Degree}°${data.tilt2_Y_Minute}'${data.tilt2_Y_Second}"`}} 
                  </v-list-tile-sub-title>
                </v-list-tile-content>
              </v-list-tile>
              <v-divider></v-divider>
              <v-list-tile>
                <v-list-tile-content>
                  <v-list-tile-title>倾角测量仪3 {{data.tilt3_Addr}}</v-list-tile-title>
                  <v-list-tile-sub-title class="caption">
                    X轴: {{`${data.tilt3_X_Positive > 0? '+' : '-'}${data.tilt3_X_Degree}°${data.tilt3_X_Minute}'${data.tilt3_X_Second}"`}} 
                    Y轴: {{`${data.tilt3_Y_Positive > 0? '+' : '-'}${data.tilt3_Y_Degree}°${data.tilt3_Y_Minute}'${data.tilt3_Y_Second}"`}} 
                  </v-list-tile-sub-title>
                </v-list-tile-content>
              </v-list-tile>
              <v-divider></v-divider>
              <v-list-tile>
                <v-list-tile-content>
                  <v-list-tile-title>倾角测量仪4 {{data.tilt4_Addr}}</v-list-tile-title>
                  <v-list-tile-sub-title class="caption">
                    X轴: {{`${data.tilt4_X_Positive > 0? '+' : '-'}${data.tilt4_X_Degree}°${data.tilt4_X_Minute}'${data.tilt4_X_Second}"`}} 
                    Y轴: {{`${data.tilt4_Y_Positive > 0? '+' : '-'}${data.tilt4_Y_Degree}°${data.tilt4_Y_Minute}'${data.tilt4_Y_Second}"`}} 
                  </v-list-tile-sub-title>
                </v-list-tile-content>
              </v-list-tile>   -->
            </v-list>          
          </v-card>
        </template>

        <v-btn absolute icon class="upbtn" color="#4A87D3" @click="uptop">
          <v-icon>arrow_upward</v-icon>
        </v-btn>
      </v-tab-item>
      <v-tab-item id="chart">
        <v-layout row class="pt-3">
          <v-flex xs6 class="px-2">
            <v-menu
              v-model="startdatemenu"
              :close-on-content-click="false"
              :nudge-right="40"
              lazy
              transition="scale-transition"
              offset-y
              full-width
            >
              <template>
                <v-text-field
                  v-model="selstartdate"
                  label="开始日期"
                  prepend-icon="event"
                  readonly
                  slot="activator"
                ></v-text-field>
              </template>
              <v-date-picker v-model="selstartdate" @input="startdatemenu = false"></v-date-picker>
            </v-menu>
          </v-flex>
          <v-flex xs6 class="px-2">
            <v-menu
              v-model="enddatemenu"
              :close-on-content-click="false"
              :nudge-right="40"
              lazy
              transition="scale-transition"
              offset-y
              full-width
            >
              <template>
                <v-text-field
                  v-model="selenddate"
                  label="结束日期"
                  prepend-icon="event"
                  readonly
                  slot="activator"
                ></v-text-field>
              </template>
              <v-date-picker v-model="selenddate" @input="enddatemenu = false"></v-date-picker>
            </v-menu>
          </v-flex>
        </v-layout>
        <v-layout row class="pb-2">
          <v-flex xs7 class="px-4 py-0">
            <v-select :items="chartOptions" item-text="text" item-value="value" v-model="selChart"></v-select>
          </v-flex>
          <v-flex xs5 class="px-2 pt-2">
            <v-btn small @click="refreshChartData">更新数据</v-btn>
          </v-flex>
        </v-layout>

        <v-card class="pa-2">
            <!-- <line-chart :chart-data="stationDatas" :options="chartOption"></line-chart> -->
          <div v-if="selChart === 1" class="chart-container" style="position: relative; height:350px; width:90vw">
            <canvas id="dataChart1"></canvas>
          </div>
          <div v-if="selChart === 2" class="chart-container" style="position: relative; height:350px; width:90vw">
            <canvas id="dataChart2"></canvas>
          </div>
          <div v-if="selChart === 3" class="chart-container" style="position: relative; height:350px; width:90vw">
            <canvas id="dataChart3"></canvas>
          </div>
          <div v-if="selChart === 4" class="chart-container" style="position: relative; height:350px; width:90vw">
            <canvas id="dataChart4"></canvas>
          </div>
        </v-card>
      </v-tab-item>
      <v-tab-item id="comments">
        <v-card v-if="!error" class="mb-2">
          <v-list dense two-line>            
            <v-list-tile>
              <v-list-tile-content>                
              </v-list-tile-content>
              <v-list-tile-action>
                <v-btn icon @click="showAddComment=true">
                  <!-- <v-icon dark>add</v-icon> -->
                  <v-badge>
                    <v-img :src="comment_add_icon" width="20px" height="20px"></v-img>
                  </v-badge>
                </v-btn>
              </v-list-tile-action>
            </v-list-tile>
          </v-list>
          <v-list three-line dense>
            <v-subheader>
                备注记录
            </v-subheader>            
            <template v-for="(comment, index) in comments">
              <v-divider
                :key="index"
              ></v-divider>
              <v-list-tile
                :key="comment.comment"
                avatar
              >
                <v-list-tile-content>
                  <v-list-tile-title><v-icon small>speaker_notes</v-icon>&nbsp;{{comment.comment}}</v-list-tile-title>
                  <v-list-tile-sub-title class="caption">{{comment.commentAt}}</v-list-tile-sub-title>
                </v-list-tile-content>
              </v-list-tile>
            </template>
          </v-list>
        </v-card>
      </v-tab-item>
    </v-tabs-items>

    <v-card v-if="error" height="100%">
      <v-card-title primary-title>
        <div>
          <h3 class="headline mb-0">
            <v-icon large color="red darken-2">error_outline</v-icon>
            {{error}}
          </h3>
        </div>
      </v-card-title>
      <v-card-text>{{this.$t("playerDetail.errorAction")}}</v-card-text>
    </v-card>
    
    <edit-station :show-edit-dialog="showEditDialog"
          :station="station"
          v-on:save="onEditStationSave" v-on:cancel="onEditStationCancel">
    </edit-station> 

    <v-dialog v-model="showAddComment" persistent max-width="85vw" style="z-index: 899">
      <v-card class="elevation-20 ">
        <v-card-title
            primary-title
          >
            添加备注
            <v-spacer></v-spacer>
        </v-card-title>
        <v-card-text style="">        
          <v-list two-line subheader dense>
            <v-list-tile class="filter_toolbar_item">
              <v-textarea rows="2" name="commentArea" placeholder="增加备注" auto-grow v-model="newComment" class="caption"></v-textarea>              
            </v-list-tile>
          </v-list>
        </v-card-text>
        <v-divider></v-divider>   
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn flat @click="showAddComment=false">取消</v-btn>
          <v-btn color="primary" flat @click="addComment">确定</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

  </v-container>
</template>

<script>
import Chart from 'chart.js';
import api from '../modules/api.js';
import EditStation from '../components/EditStation.vue';

export default {
  name: "StationDetail",
  props: ['station'],
  data() {
    return {
      error: null,
      newComment: null,
      comments: [],
      seltab: 'basic',
      coordinators: [],
      historyData: [],
      dataChart: null,
      chartOptions: [
        { text: '水平倾斜角度', value: 1 },
        { text: '垂直偏移距离', value: 2 },
        { text: '瞬时风速', value: 3 },
        { text: '环境温度', value: 4 }
      ],
      selChart: 1,
      chartOption: {
        responsive: true, 
        hover: { mode: 'nearest', intersect: true },
        showLine: false, 
        spanGaps: true, 
        steppedLine: true,
        scales: {
          xAxes: [{display: true, scaleLabel: {display: true, labelString: '日期' }}], 
          yAxes: [{display: true, scaleLabel: {display: true, labelString: '倾角(度)'}}]
        }
      },
      stationDatas: {
        labels: [],
        datasets: [{
            label: '倾角 X轴(度)', 
            backgroundColor: '#4DD0E1',
            borderColor: '#4DD0E1',
            data: [],
            fill: false
          }, {
            label: '倾角 Y轴(度)', 
            backgroundColor: '#CDDC39',
            borderColor: '#CDDC39',
            data: [],
            fill: false
          }]          
      },
      datemenu: false,
      seldate: null,
      selcoor: null,
      showEditDialog: false,
      startdatemenu: false,
      enddatemenu: false,
      selstartdate: null,
      selenddate: null,
      showAddComment: false
    };
  },
  components: {
    EditStation
  },
  watch: {
    selChart: {
      handler() {
        this.refreshChartData();
      }
    }
  },
  computed: {
    data_icon: {
      get() {
        return this.seltab === 'basic'? this.$utils.getImageUrl('data.png') : this.$utils.getImageUrl('data_disabled.png') ;
      }
    },
    history_icon: {
      get() {
        return this.seltab === 'history'? this.$utils.getImageUrl('coordinator.png') : this.$utils.getImageUrl('coordinator_disabled.png');
      }
    },
    chart_icon: {
      get() {
        return this.seltab === 'chart'? this.$utils.getImageUrl('calendar.png') : this.$utils.getImageUrl('calendar_disabled.png');
      }
    },
    comment_icon: {
      get() {
        return this.seltab === 'comments'? this.$utils.getImageUrl('comment.png') : this.$utils.getImageUrl('comment_disabled.png');
      }
    },
    map_icon: {
      get() {
        return this.$utils.getImageUrl('map.png');
      }
    },
    edit_icon: {
      get() {
        return this.$utils.getImageUrl('edit_st.png');
      }
    },
    comment_add_icon: {
      get() {
        return this.$utils.getImageUrl('comment_add.png');
      }
    }
  },
  methods: {
    goBack() {
      this.$router.go(-1);
    },    
    gotoMap() {
      if (this.station.lng && this.station.lat) {
        this.$router.push({
          name: "stations",
          params: { displayStationId: this.station.id }
        });
      } else {
        this.$utils.toast(`这个基站还没有设置地理位置`);
      }
    },
    refresh() {
      if (this.seltab === 'basic') {
        this.loadCoordinators();
      } else if (this.seltab === 'history') {
        this.loadDataByDate();
      } else if (this.seltab === 'comments') {
        this.loadComments();
      }      
    },
    gotoWarns() {
      this.$router.push({
        name: "warnsHistory",
        params: { stationId: this.station.id }
      });
    },
    addComment() {
      if (this.newComment) {
        this.showAddComment = false;
        api.addComment(this.station.id, this.newComment)
        .then(() => {
          this.$utils.toast(`添加备注成功`);
          this.newComment = null;
          this.loadComments();
        }).catch(err => {
          this.$utils.toast(`添加备注出错: ${err.message}`);
        });
      }
    },
    loadCoordinators() {
      api.getStationCoordinators(this.station.id)
      .then(result => {
        this.coordinators = result;
        this.loadLatestData();
      }).catch(err => {
        this.$utils.toast(`获取协调器信息失败: ${err.message}`);
      })
    },
    loadLatestData() {
      if (this.coordinators && this.coordinators.length > 0) {
        for (let i = 0; i < this.coordinators.length; i++) {
          let coor = this.coordinators[i];
          api.getStationDataLatest(coor.stationId, coor.seqId)
          .then(data => {
            coor.data = data;
            this.$set(this.coordinators, i, coor);
          }).catch(err => {
            this.$utils.toast(`获取最新数据失败: ${err.message}`);
          });
        }
      }
    },
    loadDataByDate() {
      if (this.seldate && this.selcoor) {
        this.historyData = [];
        this.$utils.showLoading();
        api.getStationDataByDay(this.selcoor.stationId, this.selcoor.seqId, this.seldate)
        .then(result => {
          this.$utils.hideLoading();
          this.historyData = result;
        }).catch(err => {
          this.$utils.hideLoading();
          this.$utils.toast(`获取历史数据出错: ${err.message}`);
        })
      }
    },
    loadComments() {
      this.comments = [];
      this.$utils.showLoading();
      api.getStationComments(this.station.id)
      .then(result => {
        this.$utils.hideLoading();
        this.comments = result
      }).catch(err => {
        this.$utils.hideLoading();
        this.$utils.toast(`获取基站备注信息出错: ${err.message}`);
      })
    },
    tabchange(value) {
      console.log(`Tab selected: ${value}`);
      if (this.seltab === 'comments') {
        this.loadComments();
      }
    },
    datechanged() {
      this.datemenu = false;
      console.log(this.seldate);
      this.loadDataByDate();
    },
    coorchanged() {
      console.log(this.selcoor);
      this.loadDataByDate();
    },
    coorlabel(item) {
      return `${item.seqId} - ${item.address}`;
    },
    uptop() {
      document.body.scrollTop = 0; // For Safari
      document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
    },
    editstation() {
      this.showEditDialog = true;
    },
    onEditStationSave() {
      this.showEditDialog = false;
    },
    onEditStationCancel() {
      this.showEditDialog = false;
    },
    drawChartData() {
      let cxt = document.getElementById('dataChart'+this.selChart).getContext('2d');
      let label = '倾角(度)';
      let step = 20;
      switch(this.selChart) {
        case 1: 
          label = '倾角(度)'; step = 20; break;
        case 2:
          label = '距离(毫米)'; step = 100; break;
        case 3:
          label = '风速(米/秒)'; step = 0.1; break;
        case 4:
          label = '温度(度)'; step = 5; break;
      }
      this.dataChart = new Chart(cxt, {
        type: 'line',
        data: this.stationDatas,
        options: {
          responsive: true, 
          maintainAspectRatio: false,
          hover: { mode: 'nearest', intersect: true },
          showLine: true, 
          spanGaps: true, 
          steppedLine: true,
          scales: {
            xAxes: [{display: true, scaleLabel: {display: true, labelString: '日期' }}], 
            yAxes: [{display: true, scaleLabel: {display: true, labelString: label}, ticks:{ min:0, stepSize: step }}]
          } 
        }
      });
    },
    refreshChartData() {
      this.stationDatas = null;
      switch(this.selChart) {
        case 1: 
          this.stationDatas = {
          labels: [],
          datasets: [{
              label: '倾角 X轴(度)', 
              backgroundColor: '#4DD0E1',
              borderColor: '#4DD0E1',
              data: [],
              fill: false
            }, {
              label: '倾角 Y轴(度)', 
              backgroundColor: '#CDDC39',
              borderColor: '#CDDC39',
              data: [],
              fill: false
            }]          
          };
          break;
        case 2: 
          this.stationDatas = {
          labels: [],
          datasets: [{
              label: '距离 X轴(毫米)', 
              backgroundColor: '#4DD0E1',
              borderColor: '#4DD0E1',
              data: [],
              fill: false
            }, {
              label: '距离 Y轴(毫米)', 
              backgroundColor: '#CDDC39',
              borderColor: '#CDDC39',
              data: [],
              fill: false
            }]          
          };
          break;
        case 3: 
          this.stationDatas = {
          labels: [],
          datasets: [{
              label: '风速(米/秒)', 
              backgroundColor: '#4DD0E1',
              borderColor: '#4DD0E1',
              data: [],
              fill: false
            }]          
          };
          break;
        case 4:
          this.stationDatas = {
          labels: [],
          datasets: [{
              label: '温度(度)', 
              backgroundColor: '#4DD0E1',
              borderColor: '#4DD0E1',
              data: [],
              fill: false
            }]          
          };
          break;
      }

      if (this.selstartdate && this.selenddate) {
        if (this.coordinators && this.coordinators.length > 0) {
          this.$utils.showLoading();

          api.getStationAvgDataByDate(this.station.id, this.coordinators[0].seqId, this.selstartdate, this.selenddate)
          .then(result => {
            this.$utils.hideLoading();
            if (result && result.length > 0) {
              result.forEach(r => {
                this.stationDatas.labels.push(r.date);
                if (this.selChart === 1) {
                  this.stationDatas.datasets[0].data.push(this.toFixedFloat(r.tilt_Avg_X, 2));
                  this.stationDatas.datasets[1].data.push(this.toFixedFloat(r.tilt_Avg_Y, 2));
                } else if (this.selChart === 2) {
                  this.stationDatas.datasets[0].data.push(this.toFixedFloat(r.skewing_Avg_X, 0));
                  this.stationDatas.datasets[1].data.push(this.toFixedFloat(r.skewing_Avg_Y, 0));
                } else if (this.selChart === 3) {
                  this.stationDatas.datasets[0].data.push(this.toFixedFloat(r.speed_Avg, 2));
                } else if (this.selChart === 4) {
                  this.stationDatas.datasets[0].data.push(this.toFixedFloat(r.temperature_Avg, 1));
                }                
              });
            }
            this.drawChartData();
          }).catch(err => {
            this.$utils.hideLoading();
            this.$utils.toast(`获取数据失败: ${err.message}`);
          })
        } else {
          this.$utils.toast(`该基站还没有协调器`);
        }
      } else {
        this.$utils.toast(`请先选择开始日期及结束日期`);
      }            
    },
    toNumValue(v) {
      if (v && v != null && v != undefined) {
        return v.toFixed(3);
      }
      return '';
    },
    toFixedFloat(v, num) {
      return parseFloat(v.toFixed(num));
    }
  },
  beforeMount: function() { 
    this.seldate = new Date().toISOString().substr(0, 10);   
  },
  activated: function() {    
    this.seltab = 'basic';
    this.loadCoordinators();    
  }
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
.item_value {
  align-items: flex-end;
  font-weight: 600;
}
.action {
  color: indigo;
  font-weight: 800;
}
.hot_icon {
  font-size: 14px;
}
.cus-icon {
  position: relative;
  z-index: 1;
}
.cus-icon::before {
  height: 17px;
  width: 17px;
  content: "";
  position: absolute;
  opacity: 0.54;
  z-index: -1;
  top: 0;
  left: 0;
  background-size: 17px 17px;
  background-repeat: no-repeat;
}
.upbtn {
  display: block;
  position: fixed; /* Fixed/sticky position */
  bottom: 70px; /* Place the button at the bottom of the page */
  right: 20px; /* Place the button 30px from the right */
  z-index: 99; /* Make sure it does not overlap */
  border: none; /* Remove borders */
}
</style>
