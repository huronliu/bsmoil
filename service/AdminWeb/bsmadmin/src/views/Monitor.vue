<template>
  <v-container fluid>
    <v-layout class="my-2 mx-0" column>
      <v-flex xs12>
        <v-toolbar flat class="transparent">
          <v-btn icon @click="refresh">
            <v-icon>compare_arrows</v-icon>
          </v-btn>
          <v-alert dense :type="infoType" :value="showInfo">{{infoMessage}}</v-alert>
          <v-spacer></v-spacer>
          <v-btn icon class="transparent" @click="clearDatagrams">
            <v-icon>clear</v-icon>
          </v-btn>
        </v-toolbar>        
      </v-flex>
    </v-layout>
    <v-layout row>
      <v-flex xs12 class="mx-2">
        <v-tabs
          v-model="tab"
          color="cyan lighten-2"
          grow dark slot="extension"
        >
          <v-tabs-slider color="indigo darken-4"></v-tabs-slider>
          <v-tab href="#tabs-packets" class="title">
            实时数据包
          </v-tab>
          <v-tab href="#tabs-stationdata" class="title">
            基站上报数据
          </v-tab>
          <v-tab href="#tabs-stationalert" class="title">
            基站报警数据
          </v-tab>
        </v-tabs>
      </v-flex>
    </v-layout>

    <v-tabs-items v-model="tab" class="white elevation-1 mt-1">
      <v-tab-item value="tabs-packets" key="1">
        <v-data-table :headers="datagram_headers" class="mt-2 elevation-1" hide-actions :total-items="datagram_max" :items="datagram_items">
          <template v-slot:items="props">
          <td>{{ props.item.time }}</td>
          <td class="text-xs-left">{{ props.item.type }}</td>
          <td class="text-xs-left">{{ props.item.from }}</td>
          <td class="text-xs-left">{{ props.item.bytes }}</td>
        </template>
        </v-data-table>
      </v-tab-item>
      <v-tab-item value="tabs-stationdata" key="2">
      </v-tab-item>
      <v-tab-item value="tabs-stationalert" key="3">
      </v-tab-item>
    </v-tabs-items>
  </v-container>
</template>

<script>
/* eslint-disable */
import io from 'socket.io-client';
export default {
  name: 'Monitor',
  data() {
    return {
      socket: null,
      showInfo: false,
      infoMessage: null,
      infoType: 'info',
      tab: null,
      datagram_headers: [{
        text: '时间',
        align: 'left',
        sortable: false,
        value: 'time'
      },{
        text: '命令类型',
        align: 'left',
        sortable: false,
        value: 'type'
      },{
        text: '客户端IP',
        align: 'left',
        sortable: false,
        value: 'from'
      },{
        text: '数据',
        align: 'left',
        sortable: false,
        value: 'bytes'
      }],
      datagram_max: 100,
      datagram_items: []
    }
  },
  methods: {
    displayInfo(message) {
      this.infoType = 'info';
      this.showInfo = true;
      this.infoMessage = message;
    },
    displayError(message) {
      this.infoType = 'error';
      this.showInfo = true;
      this.infoMessage = message;
    },
    displaySuccess(message) {
      this.infoType = 'success';
      this.showInfo = true;
      this.infoMessage = message;
    },
    refresh() {
      if (this.socket && this.socket.readyState === 3) {
        this.initWebSocket();
      }
    },
    clearDatagrams() {
      this.datagram_items = [];
    },
    initWebSocket() {      
      let setting = this.$ls.get('bsm_setting');
      if (setting) {
        let settingobj = JSON.parse(setting);
        let host = settingobj.host;
        let wsport = settingobj.websocketPort;
        let wsuri = `ws://${host}:${wsport}/ws`;
        this.displayInfo(`Connecting to websocket server: ${wsuri}`);
        
        this.socket = new WebSocket(wsuri);
        
        this.socket.onopen = (event) => {
          console.log('websocket connected: ' + JSON.stringify(event));
          this.displaySuccess(`websocket connected to ${wsuri}, state: ${this.socket.readyState}`);

          this.socket.send('Hello, Websocket');
        };
        this.socket.onclose = (event) => {
          console.log('websocket is closed: ' + JSON.stringify(event));
          this.displayError(`websocket is closed`);
        };
        this.socket.onerror = (event) => {
          console.error('websocket error: ', event);
          this.displayError(`websocket error`);
        };
        this.socket.onmessage = (event) => {
          let datagram = JSON.parse(event.data);
          if (datagram && datagram.type === 'datagram') {
            this.datagram_items.unshift(datagram.data);
            if (this.datagram_items.length > this.datagram_max) {
              this.datagram_items = this.datagram_items.slice(0, this.datagram_max);
            }
          }          
        }
      }
    }
  },
  mounted() {
    this.datagram_items = [];
    this.initWebSocket();
  },
  destroyed() {
    if (this.socket) {
      this.socket.close();
    }    
  }
}
</script>
