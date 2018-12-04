import Vue from "vue";
import Router from "vue-router";
import Login from "@/views/Login.vue";
import Stations from "@/views/Stations.vue";
import StationDetail from "@/views/StationDetail.vue";
import Alerts from "@/views/Alerts.vue";
import Config from "@/views/Config.vue";
import More from "@/views/More.vue";

Vue.use(Router);

export default new Router({
  mode: "history",
  routes: [
    {
      path: "/login",
      name: "login",
      component: Login,
      meta: {
        footer: false
      }
    },
    {
      path: "/",
      name: "home",
      component: Stations,
      meta: {
        requireAuth: true,
        roles: []
      }
    },
    {
      path: "/stations",
      name: "stations",
      component: Stations,
      meta: {
        requireAuth: true,
        roles: []
      }
    },
    {
      path: "/stationdetail",
      name: "stationDetail",
      component: StationDetail,
      meta: {
        requireAuth: true,
        roles: []
      },
      props: true
    },
    {
      path: "/alerts",
      name: "alerts",
      component: Alerts,
      meta: {
        requireAuth: true,
        roles: []
      }
    },
    {
      path: "/admin",
      name: "admin",
      component: Config,
      meta: {
        requireAuth: true,
        roles: []
      }
    },
    {
      path: "/admin/config",
      name: "admin/config",
      component: Config,
      meta: {
        requireAuth: true,
        roles: []
      }
    },
    {
      path: "/me",
      name: "me",
      component: More,
      meta: {
        requireAuth: true,
        roles: []
      }
    }
  ]
});
