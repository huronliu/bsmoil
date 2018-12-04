<template>
    <mobile-login class="body"
      product-name="Mobile Host"
      :hosting-mode="hostingMode"
      :show-settings="showSettings"
      login-api-path="/auth/authenticate"
      v-on:login-succeed="loginSucceed">
    </mobile-login>
</template>

<script>
import MobileLogin from "@igt/mobile-login/src/MobileLogin.vue";
import _ from "lodash";
import globalConfig from "@/modules/globalConfig";

export default {
  name: "Login",
  components: {
    MobileLogin
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
    }
  },
  methods: {
    loginSucceed(data) {
      const token = data.token;
      const user = _.omit(data, ["token", "refreshtoken", "services"]);
      this.$store.commit("setSession", { user, token });
      this.$store.commit("reloadServerSetting");

      if (!user.admin) {
        this.$router.replace("/");
      } else {
        this.$router.replace("/admin");
      }
    }
  },
  beforeRouteLeave(to, from, next) {
    console.log("beforeRouteLeave - Login", to, from);
    // Clean the global cache after each login
    // TODO:: do we need to reload the other caches after each login?
    globalConfig.clear();
    globalConfig
      .init()
      .then(() => next())
      .catch(() => window.alert("Unable to initialize the global configs"));
  }
};
</script>
<style scoped>
.body {
  background: url("../assets/background.jpg") #009adc fixed no-repeat center top;
  height: 100vh;
}
</style>
