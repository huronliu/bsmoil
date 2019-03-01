<template>
  <v-layout class="mt-3 mx-5">
    <v-layout row wrap align-start justify-start fill-height>
      <v-flex xs12 sm12>
        <v-toolbar flat class="transparent">
          <v-toolbar-title>基站管理</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-btn icon>
            <v-icon>add_circle_outline</v-icon>
          </v-btn>
          <v-btn icon @click="refresh">
            <v-icon>refresh</v-icon>
          </v-btn>
        </v-toolbar>
      </v-flex>
      <v-flex xs12 sm12>
        <v-data-table
          :headers="headers"
          :items="stations"
          class="elevation-1"
        >
          <template slot="items" slot-scope="props">
            <td>{{ props.item.name }}</td>
            <td class="text-xs-right">{{ props.item.tag }}</td>
            <td class="text-xs-right">{{ props.item.city }}</td>
            <td class="text-xs-right">{{ props.item.province }}</td>
            <td class="text-xs-right">{{ props.item.disabled }}</td>
            <td class="text-xs-right">{{ props.item.createdAt }}</td>
            <td class="text-xs-right">{{ props.item.coordinators? props.item.coordinators.length : 0 }}</td>
            <td class="text-xs-right">{{ props.item.comments? props.item.comments.length : 0 }}</td>
          </template>
        </v-data-table>
      </v-flex>
    </v-layout>   
    
  </v-layout>
</template>

<script>
import axios from 'axios';

export default {
  name: 'Stations',
  data() {
    return {
      drawer: true,
      stations: [],
      headers: [
        {text: '名称', value: 'name'} , 
        {text: '标注', value: 'tag', align: 'right'},
        {text: '城市', value: 'city', align: 'right'},
        {text: '省', value: 'province', align: 'right'},
        {text: '禁用', value: 'disabled', align: 'right'}, 
        {text: '创建时间', value: 'createdAt', align: 'right'},
        {text: '协调器', value: '', align: 'right'}, 
        {text: '备注', value: '', align: 'right'}
      ],
    }
  },
  methods: {
    refresh() {
      this.retrieveStations();
    },
    apiurl() {
      let setting = JSON.parse(this.$ls.get('bsm_setting'));
      return `http://${setting.host}:${setting.apiPort}/api`;
    },
    retrieveStations() {
      axios.get(this.apiurl() + '/stations', {
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          "Access-Control-Allow-Origin": "*",
        }, 
        data: {}
      })
      .then(result => {
        this.stations = result.data;
      }).catch(err => {
        this.$util.error(`${err.response.statusText}`);
      });
    }
  }
}
</script>

