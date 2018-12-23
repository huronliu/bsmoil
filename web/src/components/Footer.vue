<template>
  <!--bottom-nav-->
    <v-footer height="56" dark app>
      <v-bottom-nav :value="true" fixed color="primary">
        <v-btn flat
          to="stations">
          <span>基站</span>
          <v-icon>list_alt</v-icon>
        </v-btn>
        <v-btn flat
          to="warns">
          <span>报警</span>
          <v-badge color="red" v-model="showFlag">
            <span slot="badge">{{warnsCount}}</span>
            <v-icon>notification_important</v-icon>
          </v-badge>          
        </v-btn>
        <v-btn flat
          to="config">
          <span>配置</span>
          <v-icon>settings</v-icon>
        </v-btn>
        <v-btn flat
          to="me">
          <span>我</span>
          <v-icon>account_box</v-icon>
        </v-btn>
      </v-bottom-nav>
    </v-footer>
</template>

<script>
export default {
  props: {
    // navs is a list of { title: 'Hello', to: 'enroll', icon: 'person_add' }
    
  },
  data() {
    return {
      warnsCount: 5
      
    };
  },
  computed: {
    showFlag: {
      get() {
        return this.warnsCount && this.warnsCount > 0;
      }
    }
  }
};
</script>
