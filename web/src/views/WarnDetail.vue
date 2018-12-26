<template>
  <v-container fluid>
    <mobile-header title="报警数据">
      <template slot="left">
        <v-btn icon large v-on:click="goBack()">
          <v-icon medium>keyboard_arrow_left</v-icon>
        </v-btn>
      </template>
    </mobile-header>
   
    <!-- station info  -->
    <v-card v-if="!error" class="mb-1">
      <v-layout align-center >
        <v-flex>
          <v-list dense class="pl-3 pr-3">
            <v-subheader>
              站点信息
            </v-subheader>
            <v-list-tile>
              <v-list-tile-content>
                <span class="font-weight-bold">
                  <v-icon small>place</v-icon>
                  {{ warn.stationTitle }}
                </span>
              </v-list-tile-content>              
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-content>
                <span>
                  <v-icon small>perm_device_information</v-icon>
                  {{ warn.stationId }}
                </span>
              </v-list-tile-content>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-content>
                <span>
                  <v-icon small>schedule</v-icon>
                  {{ warn.time }}
                </span>
              </v-list-tile-content>
            </v-list-tile>
          </v-list>
        </v-flex>
      </v-layout>
    </v-card>

    <!-- Station -->
    <v-card v-if="!error" class="mb-2">
      <v-layout align-center>
        <v-flex>
          <v-list dense class="pl-3 pr-3">
            <v-subheader>
              报警信息
            </v-subheader>
            <v-list-tile>
              <v-list-tile-content>报警类型</v-list-tile-content>
              <v-list-tile-content class="item_value"
              >{{ warn.type }}</v-list-tile-content>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-content>报警数据</v-list-tile-content>
              <v-list-tile-content class="item_value"
              >{{ warn.data }}</v-list-tile-content>
            </v-list-tile>
                       
          </v-list>
        </v-flex>
      </v-layout>
    </v-card>

    <!-- resolution -->
    <v-card v-if="!error && warn.resolve" class="mb-2">
      <v-layout align-center>
        <v-flex>
          <v-list dense three-line class="pl-3 pr-3">
            <v-subheader>
              解决结果
            </v-subheader>
            <v-list-tile>
              <v-list-tile-content>
                <v-list-tile-title>
                  {{ warn.resolve.resolution }}
                </v-list-tile-title>
                <v-list-tile-sub-title>
                  {{ warn.resolve.user }}   
                </v-list-tile-sub-title>
              </v-list-tile-content>
              <v-list-tile-content>
                <v-list-tile-sub-title>
                  {{ warn.resolve.time }}
                </v-list-tile-sub-title>
              </v-list-tile-content>
            </v-list-tile>          
          </v-list>
        </v-flex>
      </v-layout>
    </v-card>
    
    <!-- action buttons -->
    <v-card v-if="!error" class="mb-2">
      <v-layout align-center justify-center >
        <v-flex>
          <div class="text-xs-center">
            <v-btn color="blue-grey" dark round @click.native="showResolution = !showResolution">
              解决
              <v-icon right dark>done_outline</v-icon>
            </v-btn>
            <v-btn color="blue-grey" dark round @click.native="gotoWarnsHistory">
              查看历史记录
              <v-icon right dark>history</v-icon>
            </v-btn>          
          </div>
        </v-flex>
      </v-layout>
    </v-card>

    <v-card v-if="!error && showResolution" class="mb-2">
      <v-layout align-center class="pa-3" wrap>
        <v-flex xs12>
          <v-list dense class="pl-3 pr-3">            
            <v-list-tile>
              <v-list-tile-content>
                <v-textarea name="txtresolution" label="请输入解决结果及方案" v-model="resolution"></v-textarea>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile>
              <v-list-tile-content>
                <v-btn round color="blue-grey" dark @click="submitResolution">提交</v-btn>
              </v-list-tile-content>
            </v-list-tile>
          </v-list>          
        </v-flex>
      </v-layout>
    </v-card>

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
  </v-container>
</template>

<script>

export default {
  name: "WarnDetail",
  props: ['warn'],
  data() {
    return {
      error: null,
      showResolution: false,
      resolution: null
    };
  },
  computed: {
  },
  methods: {
    goBack() {
      this.$router.go(-1);
    },    
    gotoMap() {

    },
    gotoWarnsHistory() {
      this.$router.push({
        name: "warnsHistory",
        params: { stationId: this.warn.stationId }
      });
    },
    retrieveWarnDetail(warnId) {
      this.error = null;
      this.$utils.showLoading();
      this.$utils.hideLoading();
    },
    submitResolution() {
      this.showResolution = false;
      this.warn.resolve = {
        resolution: this.resolution,
        user: "xiaodong",
        time: new Date()
      }
    }
  },
  watch: {},
  beforeMount: function() {    
  },
  activated: function() {    
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
</style>
