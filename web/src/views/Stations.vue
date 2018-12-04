<template>
  <v-container fluid>
    <mobile-header :title="pageTitle">
      <template>
        <v-btn icon large v-on:click="refresh()">
          <v-icon>refresh</v-icon>
        </v-btn>
      </template>
    </mobile-header>

    <v-card>
      <v-toolbar id="players-toolbar" dense>
        <v-text-field class="ml-2" clearable v-model="searchKey"></v-text-field>
        <v-btn icon v-on:click="search()">
          <v-icon>search</v-icon>
        </v-btn>
        <v-divider vertical class="mr-1"></v-divider>
        <div class="totalArea">
          <div>
            <v-icon class="title_icon">credit_card</v-icon>
            &nbsp;{{totalCarded}}
          </div>
          <div>
            <i class="cus-icon uncarded">&nbsp;&nbsp;&nbsp;&nbsp;</i>
            &nbsp;{{totalUncarded}}
          </div>
        </div>
        <v-divider vertical></v-divider>
        <v-menu close-delay="500" bottom left>
          <v-btn slot="activator" icon>
            <v-icon light class="ml-3 mr-3">sort_by_alpha</v-icon>
          </v-btn>
          <v-list>
            <v-list-tile @click.native="orderByClick('lastName')">
              <v-list-tile-action
                v-bind:style="{visibility: order_by_field == 'lastName' ? 'visible' : 'hidden'}"
              >
                <v-icon class="mr-3">check</v-icon>
              </v-list-tile-action>
              <v-list-tile-title>{{$t("floor.lastName")}}</v-list-tile-title>
              <v-list-tile-action>
                <v-icon
                  v-show="order_by_field == 'lastName'"
                >{{order_by_direction == "desc" ? "arrow_downward" : "arrow_upward"}}</v-icon>
              </v-list-tile-action>
            </v-list-tile>
            <v-list-tile @click.native="orderByClick('firstName')">
              <v-list-tile-action
                v-bind:style="{visibility: order_by_field == 'firstName' ? 'visible' : 'hidden'}"
              >
                <v-icon class="mr-3">check</v-icon>
              </v-list-tile-action>
              <v-list-tile-title>{{$t("floor.firstName")}}</v-list-tile-title>
              <v-list-tile-action>
                <v-icon
                  v-show="order_by_field == 'firstName'"
                >{{order_by_direction == "desc" ? "arrow_downward" : "arrow_upward"}}</v-icon>
              </v-list-tile-action>
            </v-list-tile>
            <v-list-tile @click.native="orderByClick('coinIn')">
              <v-list-tile-action
                v-bind:style="{visibility: order_by_field == 'coinIn' ? 'visible' : 'hidden'}"
              >
                <v-icon class="mr-3">check</v-icon>
              </v-list-tile-action>
              <v-list-tile-title>{{$t("playerDetail.coinIn")}}</v-list-tile-title>
              <v-list-tile-action>
                <v-icon
                  v-show="order_by_field == 'coinIn'"
                >{{order_by_direction == "desc" ? "arrow_downward" : "arrow_upward"}}</v-icon>
              </v-list-tile-action>
            </v-list-tile>
          </v-list>
        </v-menu>
        <v-dialog v-model="filterMenu" persistent max-width="280px">
          <v-btn slot="activator" icon style="font-size:18px">
            <i class="cus-icon filter">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</i>
          </v-btn>
          <v-card class="elevation-20">
            <v-list two-line subheader dense>
              <v-subheader>{{$t("floor.cardedStatus")}}</v-subheader>
              <v-list-tile class="filter_toolbar_item">
                <v-list-tile-action>
                  <v-btn-toggle v-model="filter_UI.carded">
                    <v-btn flat value="true" class="pl-2 pr-2">{{$t("floor.carded")}}</v-btn>
                    <v-btn flat value="false" class="pl-2 pr-3">{{$t("floor.unCarded")}}</v-btn>
                    <v-btn flat value="null" class="pl-2 pr-2">{{$t("floor.both")}}</v-btn>
                  </v-btn-toggle>
                </v-list-tile-action>
              </v-list-tile>
            </v-list>
            <v-list two-line subheader dense>
              <v-subheader>{{hotLabel}} {{$t("common.status")}}</v-subheader>
              <v-list-tile class="filter_toolbar_item">
                <v-list-tile-action>
                  <v-btn-toggle v-model="filter_UI.hot">
                    <v-btn flat value="true" class="pl-3 pr-3">{{hotLabel}}</v-btn>
                    <v-btn flat value="false" class="pl-4 pr-4">{{$t("common.not")}} {{hotLabel}}</v-btn>
                    <v-btn flat value="null" class="pl-3 pr-2">{{$t("floor.both")}}</v-btn>
                  </v-btn-toggle>
                </v-list-tile-action>
              </v-list-tile>
            </v-list>
            <v-list two-line subheader dense>
              <v-subheader>{{$t("floor.approachStatus")}}</v-subheader>
              <v-list-tile class="filter_toolbar_item">
                <v-list-tile-action>
                  <v-btn-toggle v-model="filter_UI.approach">
                    <v-btn flat value="false" class="pl-2 pr-4">{{$t("floor.notApproached")}}</v-btn>
                    <v-btn flat value="null" class="pl-4 pr-4">{{$t("floor.all")}}</v-btn>
                  </v-btn-toggle>
                </v-list-tile-action>
              </v-list-tile>
            </v-list>
            <v-list two-line subheader dense>
              <v-subheader>{{$t("floor.coininRange")}}</v-subheader>
              <v-list-tile class="filter_toolbar_item">
                <v-list-tile-action>
                  <v-layout row justify-space-between>
                    <v-flex xs5 md5>
                      <v-text-field
                        v-model="filter_UI.coininMin"
                        outline
                        solo
                        hide-details
                        label="Min"
                        type="number"
                        maxlength="16"
                      ></v-text-field>
                    </v-flex>
                    <v-flex xs5 md5>
                      <v-text-field
                        v-model="filter_UI.coininMax"
                        outline
                        solo
                        hide-details
                        label="Max"
                        type="number"
                        maxlength="16"
                      ></v-text-field>
                    </v-flex>
                  </v-layout>
                </v-list-tile-action>
              </v-list-tile>
            </v-list>
            <v-list two-line subheader dense>
              <v-subheader>{{$t("floor.marketAuthorizer")}}</v-subheader>
              <v-list-tile class="filter_toolbar_item">
                <v-list-tile-action>
                  <v-select
                    v-model="filter_UI.marketAuthorizer"
                    clearable
                    outline
                    solo
                    hide-details
                    :items="marketAuthorizers"
                    item-text="marketAuthorizerText"
                    item-value="userId"
                  ></v-select>
                </v-list-tile-action>
              </v-list-tile>
            </v-list>
            <v-list two-line subheader dense>
              <v-subheader>{{$t("floor.sections")}}</v-subheader>
              <v-list-tile class="filter_toolbar_item">
                <v-list-tile-action>
                  <v-select
                    v-model="filter_UI.sections"
                    clearable
                    chips
                    small-chips
                    single-line
                    outline
                    solo
                    multiple
                    hide-details
                    :items="sections"
                    item-text="section"
                    item-value="section"
                  >
                    <template slot="selection" slot-scope="{ item, index }">
                      <v-chip small v-if="index === 0">
                        <span>{{ item.section }}</span>
                      </v-chip>
                      <v-chip small v-if="index === 1">
                        <span>{{ item.section }}</span>
                      </v-chip>
                      <span
                        v-if="index === 2"
                        class="grey--text caption"
                      >+{{ filter_UI.sections.length - 2 }}</span>
                    </template>
                  </v-select>
                </v-list-tile-action>
              </v-list-tile>
            </v-list>
            <v-spacer></v-spacer>
            <v-divider></v-divider>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn flat @click="filterCancelClick()">{{$t("common.cancel")}}</v-btn>
              <v-btn color="primary" flat @click="filterApplyClick()">{{$t("common.apply")}}</v-btn>
            </v-card-actions>
          </v-card>
          <v-spacer></v-spacer>
        </v-dialog>
      </v-toolbar>
      <v-layout v-show="this.playerItems.length === 0" justify-center align-center>
        <p class="headline mt-5">{{$t("floor.noData")}}</p>
      </v-layout>
      <v-list two-line>
        <template v-for="(player, index) in playerItemsDisplayed">
          <v-list-tile
            :key="player.machineNo"
            avatar
            @click.native="showDetail(player)"
            class="playerItem"
          >
            <v-list-tile-action>
              <v-icon v-if="player.hotPlayer" color="red" class="hot_icon">whatshot</v-icon>
            </v-list-tile-action>
            <v-list-tile-avatar>
              <v-img
                :src="player.playerId > 0 ? '/asset/player/image/' + player.playerId : defaultAvatar"
                height="40"
                width="40"
              ></v-img>
            </v-list-tile-avatar>

            <v-list-tile-content>
              <v-list-tile-sub-title class="text--primary">
                <v-icon class="title_icon" color="red">place</v-icon>
                {{ player.location }}
              </v-list-tile-sub-title>
              <v-list-tile-sub-title>
                <i class="cus-icon slot">&nbsp;&nbsp;&nbsp;&nbsp;</i>
                <span class="disable_pn">{{player.machineNumber}}</span>
                &nbsp;&nbsp;
                <v-icon class="title_icon" v-if="player.carded">credit_card</v-icon>
                {{player.playerRanking}}
              </v-list-tile-sub-title>
              <v-list-tile-sub-title v-if="player.playerName">
                <v-icon class="title_icon" color="blue">person</v-icon>
                {{player.playerName}}
              </v-list-tile-sub-title>
            </v-list-tile-content>

            <v-list-tile-action>
              <v-list-tile-action-text>
                <v-layout align-center>
                  <v-flex>
                    <div class="text--primary">
                      <v-icon class="amount_icon" color="#ffd700">fas fa-coins</v-icon>
                      &nbsp;{{currencySymbol}}{{ parseInt(player.coinIn).toLocaleString() }}
                    </div>
                    <div>
                      <v-icon class="amount_icon">fas fa-calculator</v-icon>
                      &nbsp;{{currencySymbol}}{{ parseInt(player.averageBet).toLocaleString() }}
                    </div>
                  </v-flex>
                </v-layout>
              </v-list-tile-action-text>
            </v-list-tile-action>
          </v-list-tile>
          <v-divider v-if="index + 1 < playerItems.length" :key="`divider-${index}`"></v-divider>
        </template>
      </v-list>
      <v-layout v-show="hasMoreData" justify-center>
        <v-btn small flat block color="white" v-on:click="showMore()">
          <v-icon medium color="indigo">expand_more</v-icon>
        </v-btn>
      </v-layout>
    </v-card>
  </v-container>
</template>

<script>
import _ from "lodash";
import UserFilterHelper from "@/modules/userFilterHelper";

export default {
  name: "PlayerOnFloor",
  data() {
    return {
      pageTitle: "",
      currencySymbol: "$",
      searchKey: "",

      defaultAvatar: "/assets/avatar.png",

      refreshFlag: true,
      refreshInterval: 15,
      totalCarded: 0,
      totalUncarded: 0,

      playerItems: [],
      playerItemsDisplayed: [],
      pageSize: 30,

      order_by_field: "coinIn",
      order_by_direction: "desc",

      filterMenu: false,
      hotLabel: "Hot",
      marketAuthorizers: [],
      sections: [],
      filter: {
        carded: null,
        hot: null,
        approach: null,
        coininMin: null,
        coininMax: null,
        marketAuthorizer: null,
        sections: null
      },
      filter_UI: {
        carded: null,
        hot: null,
        approach: null,
        coininMin: null,
        coininMax: null,
        marketAuthorizer: null,
        sections: null
      }
    };
  },
  computed: {
    hasMoreData: {
      get() {
        if (this.playerItems.length === 0) return false;
        if (this.playerItemsDisplayed.length === this.playerItems.length)
          return false;
        return true;
      }
    }
  },
  methods: {
    search() {
      console.log("search:" + this.searchKey);
    },
    orderByClick(field) {
      if (this.order_by_field == field) {
        this.order_by_direction =
          this.order_by_direction == "asc" ? "desc" : "asc";
      } else {
        this.order_by_field = field;
        this.order_by_direction = "asc";
      }
      this.saveUserOderBySetting();
      this.orderData();
    },
    orderData() {
      console.log(
        "order data by: " + this.order_by_field + " " + this.order_by_direction
      );
      this.playerItems = _.orderBy(
        this.playerItems,
        [this.order_by_field],
        [this.order_by_direction]
      );
      this.showFirstPage();
    },
    filterUI2Model() {
      _.extend(this.filter, this.filter_UI);
    },
    filterModel2UI() {
      _.extend(this.filter_UI, this.filter);
    },
    isBlank(val) {
      if (val === null) return true;

      if (typeof val === "number") return false;

      if (typeof val === "string") {
        return val.trim().length === 0;
      }

      if (val === undefined) return true;
      if (typeof val === "boolean") return false;

      return true;
    },
    validateFilter() {
      if (!this.isBlank(this.filter_UI.coininMin)) {
        if (parseInt(this.filter_UI.coininMin) < 0) {
          return false;
        }
      }

      if (!this.isBlank(this.filter_UI.coininMax)) {
        if (parseInt(this.filter_UI.coininMax) < 0) {
          return false;
        }
      }

      if (
        !this.isBlank(this.filter_UI.coininMin) &&
        !this.isBlank(this.filter_UI.coininMax)
      ) {
        if (
          parseInt(this.filter_UI.coininMax) <
          parseInt(this.filter_UI.coininMin)
        ) {
          return false;
        }
      }

      return true;
    },
    marketAuthorizerText: item => item.firstName + " " + item.lastName,
    filterCancelClick() {
      this.filterModel2UI();
      this.filterMenu = false;
    },
    filterApplyClick() {
      if (!this.validateFilter()) {
        this.$utils.toast(this.$t("floor.invalidCoininRange"));
        return;
      }

      this.filterUI2Model();
      this.filterMenu = false;
      this.saveUserFilterSetting();
      this.loadFloorInfo();
    },
    showDetail(player) {
      this.$router.push({ name: "floorDetail", params: { player: player } });
    },
    refresh() {
      console.log("refresh");
      this.loadFloorInfo();
      window.scrollTo(0, 0);
    },
    continueRefresh(intevalSeconds) {
      if (this.refreshFlag) {
        this.refresh();
        setTimeout(() => {
          this.continueRefresh(intevalSeconds);
        }, intevalSeconds * 1000);
      }
    },
    startRefresh(intevalSeconds) {
      this.refreshFlag = true;
      setTimeout(() => {
        this.continueRefresh(intevalSeconds);
      }, intevalSeconds * 1000);
    },
    stopRefresh() {
      this.refreshFlag = false;
    },
    showFirstPage() {
      if (this.playerItems.length <= this.pageSize) {
        this.playerItemsDisplayed = this.playerItems;
      } else {
        this.playerItemsDisplayed = this.playerItems.slice(0, this.pageSize);
      }
    },
    showMore() {
      if (this.playerItemsDisplayed.length < this.playerItems.length) {
        if (
          this.playerItems.length <=
          this.playerItemsDisplayed.length + this.pageSize
        ) {
          this.playerItemsDisplayed = this.playerItems;
        } else {
          this.playerItemsDisplayed = this.playerItems.slice(
            0,
            this.playerItemsDisplayed.length + this.pageSize
          );
        }
      } else {
        this.$utils.toast(this.$t("floor.noMore"));
      }
    },
    saveUserOderBySetting() {
      UserFilterHelper.setSortBy(this.order_by_field);
      UserFilterHelper.setSortDirection(this.order_by_direction);

      UserFilterHelper.saveUserFilter();
    },
    saveUserFilterSetting() {
      UserFilterHelper.setCarded(this.filter.carded);
      UserFilterHelper.setHotPlayer(this.filter.hot);
      UserFilterHelper.setApproach(this.filter.approach);
      UserFilterHelper.setCoinMin(this.filter.coininMin);
      UserFilterHelper.setCoinMax(this.filter.coininMax);
      UserFilterHelper.setMarketAuthorizer(this.filter.marketAuthorizer);
      UserFilterHelper.setSections(this.filter.sections);

      UserFilterHelper.saveUserFilter();
    },
    restoreUserSetting() {
      this.order_by_field = UserFilterHelper.getSortBy();
      this.order_by_direction = UserFilterHelper.getSortDirection();

      this.filter.carded = UserFilterHelper.getCarded();
      this.filter.hot = UserFilterHelper.getHotPlayer();
      this.filter.approach = UserFilterHelper.getApproach();
      this.filter.coininMin = UserFilterHelper.getCoinMin();
      this.filter.coininMax = UserFilterHelper.getCoinMax();
      this.filter.marketAuthorizer = UserFilterHelper.getMarketAuthorizer();
      this.filter.sections = UserFilterHelper.getSections();

      this.filterModel2UI();
    },
    loadFloorInfo() {
      this.$utils.showLoading();
      this.$http
        .get("/api/floor-info/list" + UserFilterHelper.getParameter())
        .then(result => {
          var retData = result.data;

          this.totalCarded = 0;
          this.totalUncarded = 0;
          _.forEach(retData, value => {
            value.averageBet = parseInt(
              value.coinIn / (value.games > 0.0 ? value.games : 1)
            );
            if (value.carded) {
              this.totalCarded++;
            } else {
              this.totalUncarded++;
            }
          });
          this.playerItems = _.orderBy(
            retData,
            [this.order_by_field],
            [this.order_by_direction]
          );
          this.showFirstPage();
          this.$utils.hideLoading();
        })
        .catch(error => {
          var errorMsg = "";
          if (
            error &&
            error.response &&
            error.response.status &&
            error.response.status === 401
          ) {
            errorMsg = this.$t("floor.sessionTimeout");
          } else {
            errorMsg = this.$t("common.error") + ": " + error.message;
          }

          this.$utils.hideLoading();
          this.$utils.toast(errorMsg);
        });
    },
    loadSections() {
      this.$http
        .get("/api/floor-info/sections")
        .then(result => {
          this.sections = result.data;
        })
        .catch(error => {
          var errorMsg = this.$t("common.error") + ": " + error.message;
          this.$utils.toast(errorMsg);
        });
    },
    loadMarketAuthorizers() {
      this.$http
        .get("/api/host/mkt-authorizor/list")
        .then(result => {
          this.marketAuthorizers = result.data;
        })
        .catch(error => {
          var errorMsg = this.$t("common.error") + ": " + error.message;
          this.$utils.toast(errorMsg);
        });
    }
  },
  beforeMount: function() {
    this.pageTitle = this.$t("floor.pageTitle");
    this.hotLabel = this.$config.get("setting_responsible_gaming_hot", "Hot");
    this.refreshInterval = this.$config.get(
      "setting_floor_UI_autorefresh_interval",
      15
    );
    this.pageSize = this.$config.get("setting_floor_UI_pagesize", 30);
    this.currencySymbol = this.$config.get(
      "setting_floor_currency_symbol",
      "$"
    );
    this.loadSections();
    this.loadMarketAuthorizers();
    UserFilterHelper.loadUserFilter()
      .then(result => {
        console.log(result);
        this.restoreUserSetting();
      })
      .catch(error => {
        this.$utils.toast(error.message);
      });
  },
  activated: function() {
    this.loadFloorInfo();
    this.startRefresh(this.refreshInterval);
  },
  deactivated: function() {
    this.stopRefresh();
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
#players-toolbar >>> .v-toolbar__content {
  padding: 0px 8px 0px 8px !important;
}
.total_head {
  color: rgba(0, 0, 0, 0.54);
}
.playerItem >>> .v-list__tile {
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
.slot::before {
  background-image: url(/assets/slot.png);
  height: 14px;
  width: 14px;
  background-size: 14px 14px;
}
.slot {
  margin: 0 2px;
}
.uncarded::before {
  background-image: url(/assets/uncarded.png);
  opacity: 0.54;
  background-size: 17px 17px;
  height: 17px;
  width: 17px;
}
.filter::before {
  background-image: url(/assets/filter.png);
  background-size: 24px 24px;
  height: 24px;
  width: 24px;
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
</style>
