<template>
  <v-container fluid>
    <v-card class="mx-2 my-3">
      <v-card-title primary-title>
        <v-toolbar flat class="transparent">
          <v-toolbar-title>欢迎登录</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-btn icon @click="gosetting">
            <v-icon>settings</v-icon>
          </v-btn>        
        </v-toolbar>
      </v-card-title>
      <v-card-text>
        <v-form ref="loginform">
          <v-list>
            <v-list-tile class="my-3">
              <v-text-field label="用户名" type="text" v-model="username" :rules="[rules.required]"></v-text-field>
            </v-list-tile>
            <v-list-tile class="my-3">
              <v-text-field label="密码" type="password" v-model="password" :rules="[rules.required]"></v-text-field>
            </v-list-tile>
            <v-list-tile class="my-3">
              <v-checkbox v-model="rememberme" label="记住我的密码"></v-checkbox>
            </v-list-tile>
          </v-list>
        </v-form>        
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="primary" @click="login">
          登录
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script>
import api from '../modules/api.js';
import storage from '../modules/storage.js';
import axios from 'axios';

export default {
  name: 'Login',
  data() {
    return {
      username: null,
      password: null,
      rememberme: true,
      rules: {
        required: value => !!value || "必填"
      }
    }
  },
  methods: {
    gosetting() {
      this.$router.push('/setting');
    },
    login() {
      if (this.$refs.loginform.validate()) {
        api.login(this.username, this.password)
        .then(result => {
          if (result.token) {
            storage.setToken(result.token);
            storage.setUser(result.user);
            axios.defaults.headers.common["Authorization"] = "Bearer " + result.token; 
            
            storage.setLoginName(this.username);
            storage.setRememberMe(this.rememberme);
            if (this.rememberme) {
              storage.setPassword(this.password);
            } else {
              storage.setPassword(null);
            }

            this.$router.replace('/');
          }
        }).catch(err => {
          this.$utils.toast(`登录失败: ${err.message}`);
        });
      }
    }
  },
  activated() {
    this.rememberme = storage.getRememberMe();
    this.username = storage.getLoginName();
    if (this.rememberme) {
      this.password = storage.getPassword();
    } else {
      this.password = null;
    }
  }
}
</script>

