<template>
  <v-layout class="mt-3">
    <v-flex xs12 sm6 offset-sm3>
      <v-card>
        <v-card-title primary-title>
          <div>
            <h3 class="headline mb-0">服务器设置</h3>
          </div>
        </v-card-title>
        <v-card-text>
          <v-list dense class="px-2">
            <v-list-tile class="my-3">
              <v-text-field label="服务器地址" v-model="host">
              </v-text-field>
            </v-list-tile>
            <v-list-tile class="my-3">
              <v-text-field label="服务器端口" v-model="apiport">
              </v-text-field>
            </v-list-tile>
            <v-list-tile class="my-3">
              <v-text-field label="WebSocket端口" v-model="wsport">
              </v-text-field>
            </v-list-tile>
          </v-list>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>          
          <v-btn @click="$router.go(-1)">放弃</v-btn>
          <v-btn color="primary" @click="saveSetting">保存</v-btn>
        </v-card-actions>
      </v-card>
    </v-flex>
  </v-layout>
</template>

<script>
export default {
  name: "Setting",
  data() {
    return {
      host: 'localhost',
      apiport: '8080',
      wsport: '8089'
    }
  },  
  methods: {
    saveSetting() {
      this.$ls.set('bsm_setting', JSON.stringify({
        host: this.host,
        apiPort: this.apiport,
        websocketPort: this.wsport
      }));
      this.$util.info('Setting saved successfully');
    }
  },
  mounted: function() {
    let setting = this.$ls.get('bsm_setting');
    if (setting) {
      let settingobj = JSON.parse(setting);
      this.host = settingobj.host;
      this.apiport = settingobj.apiPort;
      this.wsport = settingobj.websocketPort;
    }
  }
}
</script>
