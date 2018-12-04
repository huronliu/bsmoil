<template>
  <v-container fluid fill-height>
    <mobile-header :title="title"></mobile-header>
    <v-layout align-center>
      <v-flex>
          <v-divider></v-divider>
          <v-list>
            <v-list-tile>
               <v-list-tile-action>
                <v-icon color="green darken-2">check_circle</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title  class="highlight">{{message}}</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
          </v-list>
          <v-divider></v-divider>
          <v-layout>
            <v-flex>
                <v-list>
                  <v-list-tile>
                    <v-list-tile-content>{{$t('common.id')}}:</v-list-tile-content>
                    <v-list-tile-content class="highlight flex-end">{{id}}</v-list-tile-content>
                  </v-list-tile>
                  <v-list-tile>
                    <v-list-tile-content>{{$t('common.name')}}:</v-list-tile-content>
                    <v-list-tile-content class="highlight flex-end">{{name}}</v-list-tile-content>
                  </v-list-tile>
                  <v-list-tile>
                    <v-list-tile-content>{{idTypeDisplayText}}:</v-list-tile-content>
                    <v-list-tile-content class="highlight flex-end">{{idNumber}}</v-list-tile-content>
                  </v-list-tile>
                </v-list>
             </v-flex>
        </v-layout>
        <v-divider></v-divider>
        <v-btn block color="primary" @click='toView()'>
          <v-icon left>account_box</v-icon>
          {{$t('enrollSuccess.viewProfile')}}
        </v-btn>
        <v-divider></v-divider>
        <v-list v-show="success">
            <v-list-tile>
               <v-list-tile-action>
                <v-icon color="green darken-2">check_circle</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title  class="highlight">{{$t('print.printSuccess')}}</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
          </v-list>
        <v-layout>
            <v-flex>
                <v-list>
                  <v-radio-group :mandatory="true" v-model="PickedPrinters">
                    <v-list-tile 
                      v-for="item in items"
                      v-bind:key="item.PrinterName">
                      <v-list-tile-action>
                        <v-radio :value="{PrinterName: item.PrinterName, PrinterType: item.PrinterType, Ranking: item.Ranking}"></v-radio>
                      </v-list-tile-action>
                      <v-list-tile-content>{{item.PrinterName}}</v-list-tile-content>
                    </v-list-tile>
                  </v-radio-group>
                </v-list>
             </v-flex>
        </v-layout>
        <v-divider v-show="showPrintButton"></v-divider>
        <v-btn v-show="showPrintButton" block color="primary" @click='doPrint()'>
          <v-icon left>print</v-icon>
          {{$t('enrollSuccess.printCard')}}
        </v-btn>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
export default {
  name: "EnrollSuccess",
  props: ["id", "name", "idNumber", "convertMode"],
  data() {
    return {
      items: [],
      success: false,
      showPrintButton: false,
      PickedPrinters: null,
      title: "",
      message: ""
    };
  },
  mounted: function() {
    this.$http
      .get("/api/print/printerlist?playerid=" + this.id)
      .then(result => {
        console.info(result);
        this.items = result.data;
        if (this.items.length == 0) {
          this.$utils.toast(this.$t("print.noPrinter"));
          return;
        } else {
          var item = this.items[0];
          this.PickedPrinters = {
            PrinterName: item.PrinterName,
            PrinterType: item.PrinterType,
            Ranking: item.Ranking
          };
          this.showPrintButton = true;
        }
      })
      .catch(error => {
        var errorMsg = this.$t("common.error") + ": " + error.message;
        this.$utils.toast(errorMsg);
      });

    this.title = this.convertMode
      ? this.$t("enroll.updatePlayer")
      : this.$t("enroll.enrollPlayer");
    this.message = this.convertMode
      ? this.$t("enrollSuccess.updateSuccess")
      : this.$t("enrollSuccess.enrollSuccess");
  },
  methods: {
    doPrint() {
      if (this.PickedPrinters == null) {
        this.$utils.toast(this.$t("print.printerNotSelect"));
        return;
      }
      this.$utils.showLoading();
      var printInfo = {
        PlayerID: this.id,
        Ranking: this.ranking,
        PrinterName: this.PickedPrinters.PrinterName,
        PrinterType: this.PickedPrinters.PrinterType
      };
      this.$http
        .post("/api/print/doprint", printInfo)
        .then(result => {
          console.info(result);
          this.$utils.hideLoading();
          if (result.data.Success == false) {
            this.$utils.toast(result.data.Message);
            return;
          }
          this.items = [];
          this.success = true;
          this.showPrintButton = false;
        })
        .catch(error => {
          var errorMsg = this.$t("common.error") + ": " + error.message;
          this.$utils.hideLoading();
          this.$utils.toast(errorMsg);
        });
    },
    toView() {
      this.$router.replace("/more");
    }
  },
  computed: {
    idTypeDisplayText: {
      get() {
        let config = this.$config.getObj("setting_default_identification_type");
        if (config) {
          return config.displayText;
        }

        return "Drivers License";
      }
    }
  }
};
</script>
<style scoped>
.highlight {
  font-weight: 600;
}
.flex-end {
  align-items: flex-end;
}
</style>
