<template>
  <v-container fluid>
    <mobile-header title="报警历史记录">
      <template slot="left">
        <v-btn icon large v-on:click="goBack()">
          <v-icon medium>keyboard_arrow_left</v-icon>
        </v-btn>
      </template>
    </mobile-header>

    <v-card class="mb-1">
    <v-list three-line>
      <v-subheader>
          {{ station.title + " - [" + station.id + "]" }}
      </v-subheader>

      <template v-for="(warn, index) in warns">
        <v-divider
          inset
          :key="index"
        ></v-divider>

        <v-list-tile
          :key="warn.id"
          avatar
        >
          <v-list-tile-avatar>
            <v-icon color="red">{{getWarnTypeIcon(warn.type)}}</v-icon>
          </v-list-tile-avatar>

          <v-list-tile-content>
            <v-list-tile-title>报警时间: {{warn.time}}</v-list-tile-title>
            <v-list-tile-sub-title>结果: {{warn.resolution}} @ {{warn.resolveTime}}<br> 
            修复者: {{warn.resolveUser}}</v-list-tile-sub-title>
          </v-list-tile-content>
        </v-list-tile>
      </template>
    </v-list>
    </v-card>
  </v-container>
</template>

<script>
export default {
  name: "WarnsHistory",
  props: ["stationId"],
  data() {
    return {
      station: {
        id: "110100001", 
        title: "清华大学"
      },
      warns: [{
        type: "倾角超标",
        time: "2018-09-09 10:23:23",
        resolution: "已修复",
        resolveTime: "2018-09-10 12:30:30",
        resolveUser: "Xiaodong Liu"
      }, {
        type: "超温",
        time: "2018-08-11 14:23:23",
        resolution: "修复冷却装置",
        resolveTime: "2018-08-13 12:30:30",
        resolveUser: "Xiaodong Liu"
      }, {
        type: "超湿",
        time: "2018-06-08 15:25:23",
        resolution: "排水",
        resolveTime: "2018-06-12 12:30:30",
        resolveUser: "Xiaodong Liu"
      }]
    };
  },
  methods: {
    goBack() {
      this.$router.go(-1);
    },
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
    }
  }
}
</script>
<style scoped>
  .container {
    padding-left: 0;
    padding-right: 0;
  }
</style>

