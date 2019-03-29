<template>
  <v-container fluid>
    <mobile-header title="服务器设置">
      <template slot="left">
        <v-btn icon large v-on:click="goBack()">
          <v-icon medium>keyboard_arrow_left</v-icon>
        </v-btn>
      </template>
    </mobile-header>
    <v-card class="mt-2">
      <v-card-text>
        <v-list>
          <v-list-tile class="my-3">
            <v-text-field label="服务器地址" type="text" v-model="apiurl" :rules="[rules.required]"></v-text-field>
          </v-list-tile>
        </v-list>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="gray darken-1" flat @click="goBack">取消</v-btn>
        <v-btn color="primary" @click="save">
          保存
        </v-btn>
      </v-card-actions>
    </v-card>
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

