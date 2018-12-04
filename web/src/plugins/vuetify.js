import Vue from "vue";
import "@/stylus/main.styl";
import Vuetify from "vuetify";
// TODO:: We should not import all the vuetify components by default;
Vue.use(Vuetify, {
  theme: {
    primary: "#1976D2",
    secondary: "#424242",
    accent: "#82B1FF",
    error: "#FF5252",
    info: "#2196F3",
    success: "#4CAF50",
    warning: "#FFC107"
  }
});
