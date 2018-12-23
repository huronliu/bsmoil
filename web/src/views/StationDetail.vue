<template>
  <v-container fluid>
    <mobile-header title="基站数据">
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
                  {{ station.title }}
                </span>
              </v-list-tile-content>              
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-content>
                <span>
                  <v-icon small>perm_device_information</v-icon>
                  {{ station.id }}
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
              <v-list-tile-content>风速</v-list-tile-content>
              <v-list-tile-content class="item_value"
              >{{ station.speed }}</v-list-tile-content>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-content>温度</v-list-tile-content>
              <v-list-tile-content class="item_value"
              >{{ station.temprature }}</v-list-tile-content>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-content>湿度</v-list-tile-content>
              <v-list-tile-content class="item_value"
              >{{ station.humidity }}</v-list-tile-content>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-content>X轴倾角</v-list-tile-content>
              <v-list-tile-content class="item_value"
              >{{ station.dipX }}</v-list-tile-content>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-content>Y轴倾角</v-list-tile-content>
              <v-list-tile-content class="item_value"
              >{{ station.dipY }}</v-list-tile-content>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-content>报警次数</v-list-tile-content>
              <v-list-tile-content class="item_value"
              >{{ station.warns }}</v-list-tile-content>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-content>最后数据时间</v-list-tile-content>
              <v-list-tile-content class="item_value">{{ station.lastDataTime }}</v-list-tile-content>
            </v-list-tile>   
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-content>最后报警时间</v-list-tile-content>
              <v-list-tile-content class="item_value">{{ station.lastWarnTime }}</v-list-tile-content>
            </v-list-tile>            
          </v-list>
        </v-flex>
      </v-layout>
    </v-card>
    
    <!-- action buttons -->
    <v-card v-if="!error" class="mb-2">
    <v-layout align-center justify-center>
      <v-flex>
        <div class="text-xs-center">
          <v-btn color="primary" class="white--text" @click.native="gotoMap">
            查看地图
            <v-icon right dark>map</v-icon>
          </v-btn>
          <v-btn color="primary" class="white--text" @click.native="gotoWarns">
            查看报警记录
            <v-icon right dark>warning</v-icon>
          </v-btn>          
        </div>
      </v-flex>
    </v-layout>
    </v-card>

    <v-card v-if="!error" class="mb-2">
      <v-list three-line dense>
      <v-subheader>
          备注记录
      </v-subheader>

      <template v-for="(comment, index) in station.comments">
        <v-divider
          :key="index"
        ></v-divider>
        <v-list-tile
          :key="comment.comment"
          avatar
        >
          <v-list-tile-content>
            <v-list-tile-title><v-icon small>speaker_notes</v-icon>&nbsp;{{comment.comment}}</v-list-tile-title>
            <v-list-tile-sub-title>{{comment.user}}</v-list-tile-sub-title>
          </v-list-tile-content>
        </v-list-tile>
      </template>
      <v-divider></v-divider>
      <v-list-tile>
          <v-list-tile-content>
            <v-textarea name="commentArea" label="增加备注" auto-grow="true" v-model="newComment"></v-textarea>
          </v-list-tile-content>
          <v-list-tile-action>
            <v-btn small fab dark color="indigo" @click="addComment">
              <v-icon dark>add</v-icon>
            </v-btn>
          </v-list-tile-action>
        </v-list-tile>
    </v-list>
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
  name: "StationDetail",
  props: ['station'],
  data() {
    return {
      error: null,
      newComment: null
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
    gotoWarns() {
      this.$router.push({
        name: "warnsHistory",
        params: { stationId: this.station.id }
      });
    },
    addComment() {
      this.station.comments.push({
        comment: this.newComment,
        user: "Xiaodong"
      });
      this.newComment = "";
    },
    retrieveSessionDetail(machineNumber, sessionStartAt) {
      this.error = null;
      this.$utils.showLoading();
      this.$http
        .get(`/api/floor-info/${machineNumber}?sessionStart=${sessionStartAt}`)
        .then(result => {
          this.$utils.hideLoading();
          this.session = result.data;
          this.session.avatar =
            this.session.playerId > 0
              ? `/asset/player/image/${this.session.playerId}`
              : "/assets/avatar.png";
          this.session.actWin =
            this.session.coinIn - this.session.coinOut - this.session.handpays;
          this.session.averageBet = parseInt(
            this.session.coinIn /
              (this.session.games > 0.0 ? this.session.games : 1)
          );
          console.log(
            "Session Detail is loaded: " + JSON.stringify(this.session)
          );
        })
        .catch(error => {
          this.$utils.hideLoading();

          if (error && error.response && error.response.status) {
            if (error.response.status === 404) {
              this.error = this.$t("playerDetail.playerLeave");
            }
          }
        });
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
