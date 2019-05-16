<template>
  <v-container style="padding: 0" fluid align-start>
    <mobile-header title="我">
      <template>
      </template>
    </mobile-header>
    <v-card class="mb-2">
      <v-list two-line>
        <v-list-tile avatar>
          <v-list-tile-avatar>
            <img src="@/assets/administrator.png">
          </v-list-tile-avatar>

          <v-list-tile-content>
            <v-list-tile-title>{{user.name}}</v-list-tile-title>
            <v-list-tile-sub-title>{{user.title}}</v-list-tile-sub-title>
          </v-list-tile-content>
        </v-list-tile>
      </v-list>
    </v-card>

    <v-card class="mb-2">
      <v-list>
        <v-list-tile>
          <v-list-tile-avatar>
            <v-icon small>playlist_add_check</v-icon>
          </v-list-tile-avatar>
          <v-list-tile-content>
            <v-list-tile-title>权限范围</v-list-tile-title>
          </v-list-tile-content>
          <v-list-tile-action>
            <v-btn icon ripple>
              <v-icon small>navigate_next</v-icon>
            </v-btn>
          </v-list-tile-action>
        </v-list-tile>
        <v-divider></v-divider>
        <v-list-tile @click="changepass">
          <v-list-tile-avatar>
            <v-icon small>vpn_key</v-icon>
          </v-list-tile-avatar>
          <v-list-tile-content>
            <v-list-tile-title>更改密码</v-list-tile-title>
          </v-list-tile-content>
          <v-list-tile-action>
            <v-btn icon ripple>
              <v-icon small>navigate_next</v-icon>
            </v-btn>
          </v-list-tile-action>
        </v-list-tile>
      </v-list>
    </v-card>

    <v-card class="mb-2">
      <v-list two-line>
        <v-list-tile @click="logout">
          <v-list-tile-avatar>
            <v-icon small>exit_to_app</v-icon>
          </v-list-tile-avatar>          
          <v-list-tile-content>
            <v-list-tile-title>退出登录</v-list-tile-title>
          </v-list-tile-content>
          <v-list-tile-action>
            <v-btn icon ripple>
              <v-icon small>navigate_next</v-icon>
            </v-btn>
          </v-list-tile-action>
        </v-list-tile>
      </v-list>
    </v-card>

    <v-dialog v-model="showPasswordDialog" persistent max-width="280px">
      <v-card>
        <v-card-title primary-title>
          <v-toolbar flat class="transparent">
            <v-toolbar-title>更改密码</v-toolbar-title>
          </v-toolbar>
        </v-card-title>
        <v-card-text>
          <v-form ref="changepassform">
            <v-list>
              <v-list-tile class="my-3">
                <v-text-field label="新密码" type="password" v-model="newpass" :rules="[rules.required]"></v-text-field>
              </v-list-tile>
              <v-list-tile class="my-3">
                <v-text-field label="确认密码" type="password" v-model="veripass" :rules="[rules.required]"></v-text-field>
              </v-list-tile>
            </v-list>
          </v-form>        
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="gray darken-1" flat @click="showPasswordDialog = false">取消</v-btn>
          <v-btn color="primary" @click="savePass">
            保存
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script>
import storage from '../modules/storage.js';
import api from '../modules/api.js';

export default {
  name: 'Me',
  data() {
    return {
      user: null,
      showPasswordDialog: false,
      newpass: null,
      veripass: null,
      rules: {
        required: value => !!value || "必填"
      }
    }
  },
  methods: {
    logout() {
      storage.cleanUserSession();
      this.$http.defaults.headers.common["Authorization"] = "";
      this.$router.replace("/login");
    },
    changepass() {
      this.newpass = null;
      this.veripass = null;
      this.showPasswordDialog = true;
    },
    savePass() {
      if (this.newpass && this.newpass === this.veripass) {
        api.changeUserPass(this.user.id, this.newpass)
        .then(() => {
          this.$utils.toast(`密码更换成功`);
          this.showPasswordDialog = false;
        }).catch(err => {
          this.$utils.toast(`更换密码出错: ${err.message}`);
        });
      }
    }
  },
  mounted() {
    this.user = storage.getUser();
  }
}
</script>

