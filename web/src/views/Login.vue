<template>
  <v-container fluid fill-height class="bg">
    <v-layout align-center justify-center>
      <v-flex xs12 sm10 md8 class="mx-1">
        <v-card v-show="!settingsMode" class="mx-auto">
          <v-card-title class="title font-weight-regular justify-space-between">
            <span>登录</span>
            <v-btn icon v-on:click="openSettings">
              <v-icon>settings</v-icon>
            </v-btn>
          </v-card-title>
          
          <v-card-text>
            <v-form ref="login">
              <v-text-field prepend-icon="person" name="login" label="用户名" :rules="usernameRules" required v-model="input.loginname"></v-text-field>
              <v-text-field prepend-icon="lock" name="password" label="密码" :rules="passwordRules" v-model="input.password" :append-icon="showPassword ? 'visibility_off' : 'visibility'" :type="showPassword ? 'text' : 'password'" @click:append="showPassword = !showPassword"></v-text-field>
            </v-form>
            <p v-show="!!errorMsg" class="mt-2 ml-1 red--text">{{errorMsg}}</p>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn ref="loginBtn" color="info" v-on:click="login" :disabled="!loginBtnEnabled">登录</v-btn>
          </v-card-actions>
        </v-card>
        <v-card v-show="settingsMode">
          <v-card-title class="title font-weight-regular justify-space-between">
            <span>设置</span>
          </v-card-title>
          <v-card-text>
            <v-form ref="settings">
              <v-text-field prepend-icon="computer" name="serverUrl" label="服务器地址" :rules="urlRules" v-model="settings.serverUrl"></v-text-field>
              <v-checkbox style="padding-left: 30px;" v-model="settings.debugMode" label="调试模式"></v-checkbox>
            </v-form>
            <p v-show="settingErrorMsg !== ''" class="mt-2 ml-1 red--text">{{settingErrorMsg}}</p>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="info" v-on:click="cancelSetting">取消</v-btn>
            <v-btn color="info" v-on:click="saveSetting">保存</v-btn>
          </v-card-actions>
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
const UrlStorageKey = "HostUrl";
const DebugKey = "DebugMode";
import globalConfig from "@/modules/globalConfig";
import _ from "lodash";

export default {
  name: "Login",
  data: function() {
    return {
      debugMode: false,
      settingsMode: false,
      usernameRules: [
        v => (!!v && v.trim().length > 0) || this.$t("username_required")
      ],
      passwordRules: [
        v => (!!v && v.trim().length > 0) || this.$t("pasword_required")
      ],
      urlRules: [
        v => (!!v && v.trim().length > 0) || this.$t("hosturl_required"),
        v => /http[s]?:\/\/[a-zA-Z0-9]+/i.test(v) || this.$t("hosturl_invalid")
      ],
      input: {
        loginname: null,
        password: null
      },
      showPassword: false,
      loggingIn: false,
      settings: {
        serverUrl: null,
        debugMode: false
      },
      loginApiPath: "/auth/authenticate",
      errorMsg: null,
      settingErrorMsg: null
    };
  },
  computed: {
    hostingMode: {
      get() {
        return window.cordova ? "cordova" : "browser";
      }
    },
    showSettings: {
      get() {
        return process.env.VUE_APP_MODE !== "production";
      }
    },
    loginBtnEnabled: {
      get() {
        return this.input.loginname && this.input.password && !this.loggingIn;
      }
    },
    serverUrl: {
      get() {
        return this.settings.serverUrl;
      },
      set(val) {
        this.settings.serverUrl = val.replace(/\/+$/, "");
      }
    }
  },
  watch: {
    input: {
      handler: function() {
        this.errorMsg = null;
      },
      deep: true
    }
  },
  methods: {
    login: function() {
      this.errorMsg = null;
      if (!this.serverUrl) {
        this.errorMsg = '服务器地址未设置，请先点击设置按钮设置服务器地址';        
        return;
      }
      if (this.$refs.login.validate()) {
        this.loggingIn = true;
        // this.$http
        //   .post(this.serverUrl + this.loginApiPath, this.input)
        //   .then(response => {
        //     this.loggingIn = false;
        //     const data = response.data;
        //     const token = data.token;
        //     const user = _.omit(data, ["token", "refreshtoken", "services"]);
        //     this.$store.commit("setSession", { user, token });
        //     this.$store.commit("reloadServerSetting");

        //     if (!user.admin) {
        //       this.$router.replace("/");
        //     } else {
        //       this.$router.replace("/admin");
        //     }
        //   })
        //   .catch(error => {
        //     this.loggingIn = false;

        //     if (error.response) {
        //       const data = error.response.data;
        //       this.processLoginError(data, error.response);
        //       this.$emit("login-failed", data, error.response);
        //     } else if (error.message) {
        //       this.errorMsg = error.message;
        //       this.$emit("login-failed", error.message, error.message);
        //     }
        //   });
        this.$store.commit('setSession', {
          token: 'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJhcHBpZCI6NCwiaWQiOjIsImF1dGhhZG1pbiI6bnVsbCwiYWRtaW4iOm51bGwsImxvZ2lubmFtZSI6IkxFTyIsImZpcnN0bmFtZSI6IkxlbyIsImxhc3RuYW1lIjoiTGlhbmciLCJleHBtaW5zIjoxNDQwLCJpYXQiOjE1NDM5MTQ3MjYsImV4cCI6MTU0NDAwMTEyNn0.aQpyERs2fatmtfWFYFgkXkNiF8IRvgdbWdBR2VOixME',
          user: {
            admin: false,
            appIconUrl: "http://localhost:8699/assets/images/svg/appid4.svg",
            cards: "",
            firstname: "Leo",
            lastname: "Liang",
            loginname: "LEO",
            roles: [],
            userId: 2
          }
        })
        this.$router.replace('/');
      } else {
        this.errorMsg = "用户名或者密码错误";
      }
    },
    beforeRouteLeave(to, from, next) {
      // Clean the global cache after each login
      // TODO:: do we need to reload the other caches after each login?
      globalConfig.clear();
      globalConfig
        .init()
        .then(() => next())
        .catch(() => window.alert("Unable to initialize the global configs"));
    },
    processLoginError: function(data, response) {
      console.error("Login failed: ", data, response);
      if (data.code) {
        switch (data.code) {
          case "Unauthorized":
            this.errorMsg = "用户没有权限";
            break;
          case "UsernameNotFound":
            this.errorMsg = "该用户不存在";
            break;
          case "InvalidUsernameOrPassword":
            this.errorMsg = "用户名或密码错误";
            break;
          case "NotFound":
            this.errorMsg = "资源未找到";
            break;
          default:
            this.errorMsg = "未知的错误";
            break;
        }
      } else {
        this.errorMsg = "服务器错误，请联系管理员查询出错原因";
      }
    },
    openSettings: function() {
      this.settings.serverUrl = this.serverUrl;
      this.settings.debugMode = this.debugMode;

      if (!this.settings.serverUrl || this.settings.serverUrl.length <= 7) {
        this.settings.serverUrl = "https://";
      }

      this.settingsMode = true;
    },
    cancelSetting: function() {
      this.settingsMode = false;      
    },
    saveSetting: function() {
      if (this.$refs.settings.validate()) {
        this.serverUrl = this.settings.serverUrl;
        this.debugMode = this.settings.debugMode;

        if (this.hostingMode === "cordova") {
          this.saveCordovaSettings();
        } else if (this.hostingMode === "browser") {
          this.saveBrowserSettings();
        } else {
          this.settingErrorMsg = `Unrecognized hostingMode ${this.hostingMode}`;
        }

        this.settingsMode = false;
      } else {
        this.settingErrorMsg = "设置错误";
      }
    },
    saveCordovaSettings: function() {
      console.log(window.cordova.platformId);
      NativeStorage.putString(
        UrlStorageKey,
        this.serverUrl,
        url => console.log("HostUrl updated: ", url),
        err => console.error("HostUrl update failed", err)
      );
      NativeStorage.putString(
        DebugKey,
        this.debugMode,
        mode => console.log("DebugMode updated: ", mode),
        err => console.error("DebugMode update failed", err)
      );
    },
    saveBrowserSettings: function() {
      window.localStorage.setItem(UrlStorageKey, this.serverUrl);
      window.localStorage.setItem(DebugKey, this.debugMode);
    },
    loadCordovaSettings: function() {
      console.log(window.cordova.platformId);
      NativeStorage.getString(
        UrlStorageKey,
        url => {
          console.log("Retrieved HostUrl: ", url);
          if (!url || url === "null") {
            this.openSettings();
          } else {
            this.serverUrl = url;
          }
        },
        err => {
          console.error("Retrieve HostUrl failed", err);
          this.openSettings();
        }
      );
      NativeStorage.getString(
        DebugKey,
        mode => {
          console.log("Retrieved DebugMode: ", mode);
          this.debugMode = mode === "true";
        },
        err => {
          console.error("Retrieve DebugMode failed", err);
          this.openSettings();
        }
      );
    },
    loadBrowserSettings: function() {
      this.serverUrl =
        window.localStorage.getItem(UrlStorageKey) || window.location.origin;      
      this.debugMode = window.localStorage.getItem(DebugKey) === "true";
    }
  },
  mounted: function() {
    // Default to 'browser' for hostingMode
    this.hostingMode = this.hostingMode || "browser";
    console.log(`cordova ready - ${typeof cordova !== "undefined"}`);
    console.log(
      `NativeStorage ready - ${typeof NativeStorage !== "undefined"}`
    );
    console.log(
      `login form in ${this.hostingMode ? "cordova" : "browser"} mode`
    );

    if (this.hostingMode === "cordova") {
      this.loadCordovaSettings();
      if (cordova.platformId === "ios") {
        // cordova-status-bar plungin does not resize document height (-20px) for status bar usage
        // $('#mainpage').css('min-height', $('#mainpage').height() - 20)
      }
    } else if (this.hostingMode === "browser") {
      this.loadBrowserSettings();
    } else {
      window.alert(`Unrecognized hostingMode ${this.hostingMode}`);
    }
  }
};
</script>

<style scoped>
.bg {
  background: url("../assets/login-background.jpg") fixed repeat center top;
  height: 100vh;
}
</style>
