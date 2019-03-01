<template>
  <v-app>
      <v-toolbar color="grey darken-2" dark>    
      <v-icon>wifi_tethering</v-icon>
      <v-toolbar-title class="white--text">基站管理监控</v-toolbar-title>
      <template v-for="(item, index) in menus">
        <v-btn flat class="ml-5" :class="menustyle(item.path)" @click="clickmenu(item)" :key="index">{{item.title}}</v-btn> 
      </template>
      
      <v-spacer></v-spacer>
      <v-menu open-on-hover bottom offset-y>
        <v-toolbar-items
          slot="activator"
        >
          <v-btn icon>
            <v-icon>more_vert</v-icon>
          </v-btn>
        </v-toolbar-items>

        <v-list light>
          <v-list-tile
            @click="clickSetting"
          >
            <v-list-tile-action>
              <v-icon>settings</v-icon>
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title>服务器设置</v-list-tile-title>
            </v-list-tile-content>              
          </v-list-tile>
        </v-list>
      </v-menu>      
    </v-toolbar>

    <router-view/>

    <v-snackbar v-model="snackbar.display" :timeout="snackbar.timeout" :class="snackbarcolor">
      {{snackbar.text}}
      <v-btn color="primary" flat @click="snackbar.display = false">X</v-btn>
    </v-snackbar>
  </v-app>
</template>

<script>
export default {
  name: "App",
  data() {
    return {
      selmenu: null,
      menus: [
        { path: '/stations', title: '基站管理' },
        { path: '/monitor', title: '数据监控'}
      ],
      snackbar: {
        display: false,
        text: null,
        timeout: 30000,
        type: 'info'
      }
    }
  },
  computed: {
    snackbarcolor: {
      get() {
        if (this.snackbar.type == 'info') return 'white--text';
        if (this.snackbar.type == 'error') return 'read--text';
        return 'white--text';
      }
    }
  },
  methods: {
    clickSetting() {
      this.selmenu = 'setting';
      this.$router.push('/setting');
    },   
    clickmenu(menu) {
      this.selmenu = menu.path;
      this.$router.push(menu.path);
    },
    menustyle(menuitem) {
      if (this.selmenu === menuitem) {
        return 'yellow--text font-weight-regular subheading';
      }
      return 'white--text font-weight-thin body-1';
    }
  },
  created: function() {
    this.$bus.$on("snackbar", (arg) => {
      this.snackbar = {
        text: arg.text,
        timeout: 30000,
        display: true,
        type: arg.type
      };
    });
  }
}
</script>
