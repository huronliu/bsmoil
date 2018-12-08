<template>
  <!--bottom-nav-->
    <v-footer height="56" dark app>
      <v-bottom-nav :value="true" fixed color="primary">
        <v-btn flat
          v-for="(nav, index) in navs"
          v-bind:key="nav.title"
          v-bind:index="index"
          :to="{name: nav.to}">
          <span>{{nav.title}}</span>
          <v-icon>{{nav.icon}}</v-icon>
        </v-btn>
      </v-bottom-nav>
    </v-footer>
</template>

<script>
export default {
  props: {
    // navs is a list of { title: 'Hello', to: 'enroll', icon: 'person_add' }
    navs: {
      type: Array,
      required: true
    }
  },
  data() {
    return {};
  }
};
</script>
