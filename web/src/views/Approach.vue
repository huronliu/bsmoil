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
    
    <v-card class="mb-2 sticky">
      <v-layout align-center>
        <v-flex xs1>
        </v-flex>
        <v-flex xs3 class="py-2">
          <img :src="player.playerId > 0 ? '/asset/player/image/' + player.playerId : '/assets/unknown-user.png'" height="48" width="48" lazy-src="defaultAvatar">
        </v-flex>
        <v-flex xs8>
          <v-card-title>
            <div class="caption font-weight-thin" v-if="player.carded">
              <div>
                <span class="playerName" >{{player.playerName || 'Unknown'}}</span> 
                &nbsp; 
                <span v-if="player.hotPlayer">
                  <v-icon  color="red" small>whatshot</v-icon>
                </span> 
              </div>
              <div>
                {{$t("playerDetail.coinIn")}}: &nbsp; {{player.coinIn}}
              </div>
              <div>
                {{player.playerId}} &nbsp;  <v-icon small>credit_card</v-icon>&nbsp; {{player.playerRanking}}
              </div>
              <div>
                <span ><v-icon small >place</v-icon> {{player.location}}
                </span>  
              </div>              
            </div>
            <div class="caption font-weight-thin" v-if="!player.carded">
              <div >
                <span class="playerName">{{$t("playerDetail.uncardedPlayer")}}</span>
              </div>
              <div>
                <span>
                  <v-icon  v-if="player.hotPlayer" color="red" small>whatshot</v-icon> 
                  {{$t("playerDetail.coinIn")}}: &nbsp; {{player.coinIn}}
                </span> 
              </div>
              <div>
                <span ><v-icon small >place</v-icon> {{player.location}}
                </span>  
              </div>              
            </div>
          </v-card-title>
        </v-flex>
      </v-layout>
    </v-card>
      
    <v-card>
      <v-layout align-center>
        <v-flex xs-12>
          <v-list>
            <v-subheader>
                Approach Options:
            </v-subheader>

            <template v-for="(option, index) in options">   
              <v-list-tile
                :key="option.option"
                avatar
                @click="process(option)">
                <!-- <v-list-tile-action> -->
                  <!-- <i v-if="option.icon === 'uncarded'" class="cus-icon uncarded">&nbsp;&nbsp;&nbsp;&nbsp;</i>
                  <v-icon v-else-if="option.icon">{{option.icon}}</v-icon>
                  <v-icon v-else>touch_app</v-icon>                 -->
                <!-- </v-list-tile-action> -->
                <v-list-tile-content>
                  <v-list-tile-title v-html="option.option" class="text-uppercase"></v-list-tile-title>
                </v-list-tile-content>
                <v-list-tile-action>
                  <v-icon>chevron_right</v-icon>
                </v-list-tile-action>
              </v-list-tile>
              <v-divider v-if="index + 1 < options.length" :key="`divider-${index}`" class="mx-2"></v-divider>
            </template>
          </v-list>
        </v-flex>        
      </v-layout>
    </v-card>
  </v-container>
</template>

<script>
export default {
  name: "Approach",
  props: ["player"],
  data() {
    return {
      pageTitle: "",
      options: [],
      approachOptions: []
    };
  },
  computed: {},
  methods: {
    goBack() {
      this.$router.go(-1);
    },
    process(opt) {
      console.log("Process approach on player: " + JSON.stringify(opt));
      if (opt) {
        this.$http
          .post("/api/approach/add", {
            Location: this.player.location,
            SessionStart: this.player.sessionStartAt,
            PlayerId: this.player.playerId,
            Reason: opt.option
          })
          .then(res => {
            if (res.status === 200) {
              this.$utils.toast("Approach success");
            }
            this.$router.replace("/floor");
          })
          .catch(err => {
            this.$utils.hideLoading();
            let errorMsg = "";
            if (err && err.response && err.response.status) {
              if (err.response.status === 401) {
                errorMsg = this.$t("floor.sessionTimeout");
              }
            } else {
              errorMsg = this.$t("common.error") + ": " + err.message;
            }

            this.$utils.toast(errorMsg);
            this.$router.replace("/floor");
          });
      }
    },
    filterOptions() {
      this.options = [];
      this.options = this.approachOptions.filter(opt => {
        return (
          opt != null &&
          (opt.carded == null || opt.carded == this.player.carded)
        );
      });
      console.log("approach options: " + JSON.stringify(this.options));
    }
  },
  watch: {},
  beforeMount: function() {
    this.pageTitle = this.$t("approach.pageTitle");
    this.$utils.showLoading();
    this.$http
      .get("/api/approach/options")
      .then(result => {
        this.$utils.hideLoading();
        this.approachOptions = result.data;
        this.filterOptions();
      })
      .catch(error => {
        this.$utils.hideLoading();

        var errorMsg = "";
        if (error && error.response && error.response.status) {
          if (error.response.status === 401) {
            errorMsg = this.$t("floor.sessionTimeout");
          }
        } else {
          errorMsg = this.$t("common.error") + ": " + error.message;
        }

        this.$utils.toast(errorMsg);
      });
  },
  activated: function() {
    console.log("Approach the player: " + JSON.stringify(this.player));
    this.filterOptions();
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
.sticky {
  position: relative;
}
.cus-icon {
  position: relative;
  z-index: 1;
}
.cus-icon::before {
  height: 24px;
  width: 24px;
  content: "";
  position: absolute;
  opacity: 0.54;
  z-index: -1;
  top: 0;
  left: 0;
  background-size: 24px 24px;
  background-repeat: no-repeat;
}
.uncarded::before {
  background-image: url(/assets/uncarded.png);
}
</style>
