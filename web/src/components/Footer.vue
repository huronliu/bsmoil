<template>
  <!--bottom-nav-->
    <v-footer height="56" app >
      <v-bottom-nav :value="true" class="grey lighten-3" :active.sync="activetitle">
        <v-btn flat v-for="(nav, index) in navs" 
          v-bind:key="nav.title"
          v-bind:index="index"
          :to="nav.to" 
          style="color:#4A87D3"
          :value="nav.title">
          <span class="caption font-weight-medium">{{nav.title}}</span>
          <v-badge color="red" v-model="nav.flags">
            <span slot="badge">{{nav.flags}}</span>
            <!-- <v-icon>{{nav.icon}}</v-icon> -->
            <v-img :src="nav.title === activetitle? nav.icon : nav.disable_icon" width="24px" height="24px" class="mb-1"></v-img>
          </v-badge>                    
        </v-btn>        
      </v-bottom-nav>
    </v-footer>
</template>

<script>
export default {
  props: {
    // navs is a list of { title: 'Hello', to: 'enroll', icon: 'person_add' }    
  },
  data() {
    return {
      activetitle: '',
      warnsCount: 0,
      navs: [
        { title: "基站", to: { path: "/stations" }, icon: "station.png", disable_icon: "station_disabled.png", flags: 0, roleFlag: 1 },
        { title: "报警", to: { path: "/warns" }, icon: "warn.png", disable_icon: "warn_disabled.png", flags: 0, roleFlag: 1 },
        { title: "配置", to: { path: "/config" }, icon: "setting.png", disable_icon: "setting_disabled.png", flags: 0, roleFlag: 1 },
        { title: "我", to: { path: "/me" }, icon: "me.png", disable_icon: "me_disabled.png", flags: 0, roleFlag: 1 }    
      ]
    };
  },
  computed: {    
  },
  beforeMount() {
    this.navs.forEach(nav => {
      nav.icon = this.$utils.getBaseUrl() + "images/" + nav.icon;
      nav.disable_icon = this.$utils.getBaseUrl() + "images/" + nav.disable_icon;
    });
    this.activetitle = this.navs[0].title;
  }
};
</script>
