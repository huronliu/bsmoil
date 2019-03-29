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
        
        <v-divider vertical class="mr-1"></v-divider>        
          
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
                      v-model="filter.warnTypes"
                      clearable
                      chips
                      small-chips
                      single-line
                      multiple
                      label="报警类型"
                      hide-details
                      :items="warnTypes"
                      item-value="id"
                      item-text="type"
                    >
                      <template slot="selection" slot-scope="{ item, index }">
                        <v-chip small v-if="index === 0">
                          <span>{{ item.type }}</span>
                        </v-chip>
                        <v-chip small v-if="index === 1">
                          <span>{{ item.type }}</span>
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

      <v-layout row wrap justify-center align-center>
        <v-flex xs12>
          <v-layout v-show="this.warns.length === 0" justify-center>
            <p class="headline mt-5">没有数据</p>
          </v-layout>
        </v-flex>
        <v-flex xs12>
            <v-list two-line dense>
              <template v-for="(warn, index) in warns">
                <v-list-tile                  
                  :key="warn.id"
                  avatar
                  @click.native="showDetail(warn)"
                  class="stationItem"
                >
                  <v-list-tile-avatar>
                    <v-icon color="red">{{getWarnTypeIcon(warn.type)}}</v-icon>
                  </v-list-tile-avatar>
                  
                  <v-list-tile-content>
                    <v-list-tile-title class="text--primary">
                      <v-icon class="title_icon">place</v-icon>
                      {{ warn.stationTitle + ' (' + warn.stationId + ')' }}
                    </v-list-tile-title>
                    <v-list-tile-sub-title>
                      <span class="disable_pn">{{ warn.time }}</span>
                    </v-list-tile-sub-title>
                  </v-list-tile-content>

                  <v-list-tile-action>
                    <v-btn icon ripple>
                      <v-icon small>navigate_next</v-icon>
                    </v-btn>
                  </v-list-tile-action>                  
                </v-list-tile>
                <v-divider v-if="index + 1 < warns.length" :key="`divider-${index}`"></v-divider>
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
          
    </v-card>
  </v-container>
</template>

<script>
import moment from 'moment';

export default {
  name: "Warns",
  data() {
    return {
      searchKey: "",
      filter: {},
      city: '北京',

      refreshFlag: true,
      refreshInterval: 15,
      pageSize: 30,

      filterMenu: false,
      warns: [], 
      curWarn: null, 
      warnTypes: [
        {id: 1, type: '倾角超标'},
        {id: 2, type: '超温'},
        {id: 3, type: '超湿'},
        {id: 4, type: '烟雾浓度超标'}, 
        {id: 5, type: '断电报警'},
      ]
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
        return '报警 - ' + this.city;
      }
    }
  },
  methods: {
    getWarnTypeIcon(typeid) {
      switch(typeid) {
        case "倾角超标": 
          return "transit_enterexit";
        case "超温":
          return "whatshot";
        case "超湿":
          return "opacity";
        case "烟雾浓度超标":
          return "smoke_free";
        case "断电报警":
          return "battery_alert";
      }
      return "priority_high";
    },
    //filter menu
    filterCancelClick() {
      this.filterMenu = false;
    },
    filterApplyClick() {
      this.filterMenu = false;
    },

    //handle when click the station marker on map or in the list
    showDetail(warn) {
      this.$router.push({
        name: "warnDetail",
        params: { warn: warn }
      });
    },

    //reload stations
    refresh() {
      console.log("refresh");
      this.loadWarns();
      window.scrollTo(0, 0);
    },
    showFirstPage() {
      if (this.warns.length <= this.pageSize) {
        this.warns = this.warns;
      } else {
        this.warns = this.warns.slice(0, this.pageSize);
      }
    },
    showMore() {
      this.$utils.toast("没有更多");
    },

    //load stations from server
    loadWarns() {
      this.$utils.showLoading();
      this.warns = [];
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

    }

  },
  mounted: function() {
    
  },
  activated: function() {
    this.loadWarns();
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
