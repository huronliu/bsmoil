<template>
  <!--bottom-nav-->
    <v-footer height="56" app >
      <v-bottom-nav :value="true" class="grey lighten-3">
        <v-btn flat v-for="(nav, index) in navs" 
          v-bind:key="nav.title"
          v-bind:index="index"
          :to="nav.to">
          <span class="caption font-weight-medium">{{nav.title}}</span>
          <v-badge color="red" v-model="nav.flags">
            <span slot="badge">{{nav.flags}}</span>
            <v-icon>{{nav.icon}}</v-icon>
          </v-badge>                    
        </v-btn>        
      </v-bottom-nav>
    </v-footer>
</template>

<script>
export default {
  props: {
    // navs is a list of { title: 'Hello', to: 'enroll', icon: 'person_add' }
    navs: {
      type: Array, required: true 
    }
  },
  data() {
    return {
      warnsCount: 5      
    };
  },
  computed: {    
  }
};
</script>
