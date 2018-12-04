<template>
  <v-container fluid>
    <mobile-header :title="pageTitle">
      <template slot="left">
        <v-btn icon large v-on:click="goBack()">
          <v-icon medium>keyboard_arrow_left</v-icon>
        </v-btn>
      </template>
    </mobile-header>
    <!-- Player info card -->
    <v-card v-if="!error" class="mb-2">
      <v-layout align-center>
        <v-flex xs1>
        </v-flex>
        <v-flex xs3 class="py-2">
          <img :src="session.playerId > 0 ? '/asset/player/image/' + session.playerId : '/assets/unknown-user.png'" height="48" width="48" lazy-src="defaultAvatar">
        </v-flex>
        <v-flex xs8>
          <v-card-title>
            <div class="caption font-weight-thin" v-if="session.carded">
              <div >
                <span class="playerName" >{{session.playerName || 'Unknown'}}</span> 
                &nbsp; 
                <span v-if="session.hotPlayer">
                  <v-icon  color="red" class="hot_icon">whatshot</v-icon>                                 
                  {{ formatHotDuration(session.hotDuration) }}
                </span> 
              </div>
              <div>
                {{session.playerId}} &nbsp;  <v-icon small>credit_card</v-icon>&nbsp; {{session.playerRanking}}
              </div>
              <div>
                Theo. Win: {{currencySymbol}}{{isNumber(session.actWin)? parseInt(session.theoWin).toLocaleString(): ''}} 
              </div>
              <div v-if="session.hostName">
                <v-icon small>person</v-icon> &nbsp; {{session.hostName}}
              </div>
            </div>
            <div class="caption font-weight-thin" v-if="!session.carded">
              <div >
                <span class="playerName" v-if="!session.carded">{{$t("playerDetail.uncarded")}}</span>
              </div>
              <div>
                <span v-if="session.hotPlayer">
                  <v-icon  color="red" class="hot_icon">whatshot</v-icon>                                 
                  {{ formatHotDuration(session.hotDuration) }}
                </span> 
              </div>
            </div>
          </v-card-title>
        </v-flex>
      </v-layout>
    </v-card>
    
    <!-- Play session data -->
    <v-card v-if="!error" class="mb-2">
      <v-layout align-center v-if="!error">      
        <v-flex>
          <v-list dense class="pl-3 pr-3">              
            
            <v-list-tile>
              <v-list-tile-content>{{$t("playerDetail.coinIn")}}</v-list-tile-content>
              <v-list-tile-content class="item_value" v-if="isNumber(session.coinIn)">{{currencySymbol}}{{ parseInt(session.coinIn).toLocaleString() }}</v-list-tile-content>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-content>{{$t("playerDetail.actualWin")}}</v-list-tile-content>
              <v-list-tile-content class="item_value" v-if="isNumber(session.actWin)">{{currencySymbol}}{{parseInt(session.actWin).toLocaleString()}}</v-list-tile-content>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile>
              <v-list-tile-content>{{$t("playerDetail.averageBet")}}</v-list-tile-content>
              <v-list-tile-content class="item_value" v-if="isNumber(session.averageBet)">{{currencySymbol}}{{ parseInt(session.averageBet).toLocaleString() }}</v-list-tile-content>
            </v-list-tile>
            <v-divider></v-divider>           
            <v-list-tile>
              <v-list-tile-content>{{$t("playerDetail.gamesPlayed")}}</v-list-tile-content>
              <v-list-tile-content class="item_value">{{session.games}}</v-list-tile-content>
            </v-list-tile>
            
          </v-list>
        </v-flex>
      </v-layout>
    </v-card>
    <!-- Floor data  -->
    <v-card v-if="!error" class="mb-1">
      <v-layout align-center v-if="!error">      
        <v-flex>
          <v-list dense class="pl-3 pr-3">
            <v-list-tile>
              <v-list-tile-content>
                <span class="font-weight-bold"><v-icon small >place</v-icon> {{session.location}}
                </span>
                </v-list-tile-content>
              <v-list-tile-content class="item_value">
                <span class="font-weight-bold"><i class="cus-icon slot">&nbsp;&nbsp;&nbsp;&nbsp;</i> {{session.machineNumber}}
                  </span>
              </v-list-tile-content>
            </v-list-tile>
            <v-divider></v-divider>            
            <v-list-tile>
              <v-list-tile-content>
                <span><v-icon small >monetization_on</v-icon>
                  {{$t("playerDetail.denom")}}
                </span>
              </v-list-tile-content>
              <v-list-tile-content class="item_value">{{currencySymbol}}{{session.denom}}</v-list-tile-content>
            </v-list-tile>          
          </v-list>
        </v-flex>
      </v-layout>
    </v-card>

    <!-- action buttons -->
    
      <v-layout align-center justify-center v-if="!error">
      <v-flex>
        <div class="text-xs-center">
          <v-btn
            color="primary"
            class="white--text"
            @click.native="approach(session)"
          >
            {{$t("playerDetail.approach")}}
            <v-icon right dark>touch_app</v-icon>
          </v-btn>
          <v-btn
            color="primary"
            class="white--text"
            @click.native="enroll(session)"
            v-if="!session.carded"
          >
            {{$t("playerDetail.enroll")}}
            <v-icon right dark>person_add</v-icon>
          </v-btn>          
        </div>
      </v-flex>
    </v-layout>
        
    <v-card v-if="error" height="100%">   
        <v-card-title primary-title>
          <div>            
            <h3 class="headline mb-0"><v-icon large color="red darken-2">error_outline</v-icon> {{error}}</h3>
          </div>
        </v-card-title>
        <v-card-text>
          {{this.$t("playerDetail.errorAction")}}
        </v-card-text>

    <v-card-actions>
      <v-spacer></v-spacer>
      <v-icon
        large
        @click="next"
      >
        mdi-chevron-left
      </v-icon>
    </v-card-actions>
   </v-card>
  </v-container>
</template>

<script>
import moment from "moment";

export default {
  name: "PlayerDetail",
  props: ["player"],
  data() {
    return {
      pageTitle: "",
      currencySymbol: "$",
      session: {},
      error: null
    };
  },
  computed: {
    cardStatus: {
      get() {
        return this.session.carded
          ? this.$t("playerDetail.carded")
          : this.$t("playerDetail.uncarded");
      }
    }
  },
  methods: {
    goBack() {
      this.$router.go(-1);
    },
    enroll(player) {
      console.log("Enroll the player: " + JSON.stringify(player));
      this.$router.push({ name: "enroll" });
    },
    approach(player) {
      this.$router.push({ name: "approach", params: { player: player } });
    },
    retrieveSessionDetail(player) {
      this.error = null;
      this.$utils.showLoading();
      this.$http
        .get(
          "/api/floor-info/" +
            player.machineNumber +
            "?sessionStart=" +
            player.sessionStartAt
        )
        .then(result => {
          this.$utils.hideLoading();
          this.session = result.data;
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

          var errorMsg = "";
          if (error && error.response && error.response.status) {
            if (error.response.status === 401) {
              errorMsg = this.$t("floor.sessionTimeout");
            } else if (error.response.status === 404) {
              errorMsg =
                this.$t("playerDetail.playerLeave") +
                " or " +
                this.$t("playerDetail.playerApproached");
            }
          } else {
            errorMsg = this.$t("common.error") + ": " + error.message;
          }

          this.error = errorMsg;
        });
    },
    isNumber(n) {
      return !isNaN(parseFloat(n));
    },
    formatHotDuration(seconds) {
      if (seconds) {
        let v = moment.duration(seconds, "seconds");
        let str = "";
        if (v.hours()) {
          str += v.hours() + "h";
        }
        if (v.minutes()) {
          str += v.minutes() + "'";
        }
        if (v.seconds()) {
          str += v.seconds() + '"';
        }
        if (v.days() > 0) {
          str = v.days() + " days " + str;
        }
        return str;
      }
      return "";
    }
  },
  watch: {},
  beforeMount: function() {
    this.pageTitle = this.$t("playerDetail.pageTitle");
    this.currencySymbol = this.$config.get(
      "setting_floor_currency_symbol",
      "$"
    );
  },
  activated: function() {
    //this.session = {};
    this.error = null;
    if (this.player) {
      this.retrieveSessionDetail(this.player);
    } else if (this.session) {
      this.retrieveSessionDetail(this.session);
    } else {
      this.error = "No player session";
    }
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
.playerName {
  font-weight: 600;
}
.item_value {
  align-items: flex-end;
  font-weight: 600;
}
#players-toolbar >>> .v-toolbar__content {
  height: 40px !important;
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
.slot::before {
  background-image: url(/assets/slot.png);
  height: 14px;
  width: 14px;
  background-size: 14px 14px;
}
.slot {
  margin: 0 2px;
}
</style>
