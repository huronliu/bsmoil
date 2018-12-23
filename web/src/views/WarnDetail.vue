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
    
    <!-- action buttons -->
    <v-layout align-center justify-center v-if="!error">
      <v-flex>
        <div class="text-xs-center">
          <v-btn color="primary" class="white--text" @click.native="gotoMap">
            已解决
            <v-icon right dark>done_outline</v-icon>
          </v-btn>
          <v-btn color="primary" class="white--text" @click.native="gotoWarnsHistory">
            查看历史记录
            <v-icon right dark>history</v-icon>
          </v-btn>          
        </div>
      </v-flex>
    </v-layout>

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
      error: null
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
