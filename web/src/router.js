import Vue from "vue";
import Router from "vue-router";
import Stations from "./views/Stations";
import StationDetail from "./views/StationDetail";
import Temp from "./views/Temp";

Vue.use(Router);

export default new Router({
  mode: "history",
  routes: [
    {
      path: "/login",
      name: "login",
      component: Temp,
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
      component: Temp,
      meta: {
        requireAuth: true,
        roles: []
      }
    },
    {
      path: "/admin",
      name: "admin",
      component: Temp,
      meta: {
        requireAuth: true,
        roles: []
      }
    },
    {
      path: "/admin/config",
      name: "admin/config",
      component: Temp,
      meta: {
        requireAuth: true,
        roles: []
      }
    },
    {
      path: "/me",
      name: "me",
      component: Temp,
      meta: {
        requireAuth: true,
        roles: []
      }
    }
  ]
});
