<template>
  <v-container fluid>
    <mobile-header title="服务器设置">
      <template slot="left">
        <v-btn icon large v-on:click="goBack()">
          <v-icon medium>keyboard_arrow_left</v-icon>
        </v-btn>
      </template>
    </mobile-header>
    <v-layout justify-center wrap class="pa-2">
      <v-flex xs12 class="my-3 py-2">
        <v-text-field label="服务器地址" type="text" v-model="apiurl" :rules="[rules.required]"></v-text-field>
      </v-flex>
      <v-flex xs6 class="px-2">
        <v-btn class="grey lighten-3" flat @click="goBack" block>取消</v-btn>
      </v-flex>
      <v-flex xs6 class="px-2">
        <v-btn class="primary" @click="save" block>
          保存
        </v-btn>
      </v-flex>
    </v-layout>
  </v-container>
</template>
<script>
import storage from '../modules/storage.js';
export default {
  name: "Setting",
  data() {
    return {
      apiurl: null,
      rules: {
        required: value => !!value || "必填"
      }
    }
  },
  methods: {
    goBack() {
      this.$router.go(-1);
    }, 
    save() {
      storage.setServiceUrl(this.apiurl);
      this.goBack();
    }
  },
  activated() {
    this.apiurl = storage.getServiceUrl();
  }
}
</script>

