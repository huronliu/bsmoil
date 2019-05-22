<template>
  <v-container align-center justify-center style="min-height:100vh;" class="main_color pa-2 ma-0">
    <v-layout justify-end>
      <v-flex xs2>
        <v-btn icon @click="gosetting" dark>
          <v-icon dark>settings</v-icon>
        </v-btn>
      </v-flex>
    </v-layout>
    <v-layout justify-center>
      <v-flex xs8>
        <v-img :src="loginimg"></v-img>
      </v-flex>
    </v-layout>
    
    <v-form ref="loginform">
      <v-layout justify-center wrap class="px-4">
        <v-flex xs12>
          <v-text-field class="main_color" dark placeholder="用户名" type="text" prepend-inner-icon="perm_identity" v-model="username" :rules="[rules.required]" single-line></v-text-field>
        </v-flex>
        <v-flex xs12>
          <v-text-field class="main_color" dark placeholder="密码" type="password" prepend-inner-icon="lock_open" v-model="password" :rules="[rules.required]" single-line></v-text-field>
        </v-flex>
        <v-flex xs12>
          <v-checkbox dark v-model="rememberme" label="记住我的密码"></v-checkbox>
        </v-flex>
        <v-flex xs12>
          <v-btn color="grey lighten-4" @click="login" block>
            登录
          </v-btn>
        </v-flex>
      </v-layout>              
    </v-form>        
    
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
  computed: {
    loginimg: {
      get() {
        return this.$utils.getBaseUrl() + "images/mmexport1547008523796.jpg";
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
  },
  mounted() {
  }
}
</script>

