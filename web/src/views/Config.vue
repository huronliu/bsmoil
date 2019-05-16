<template>
  <v-container fluid class="ma-0 pa-0">
    <mobile-header title="配置">
      <template>
      </template>
    </mobile-header>

      <v-list two-line>
        <v-list-tile @click="setting">
          <v-list-tile-avatar>
            <v-icon >settings</v-icon>
          </v-list-tile-avatar>          
          <v-list-tile-content>
            <v-list-tile-title>服务器配置</v-list-tile-title>
          </v-list-tile-content>
          <v-list-tile-action>
            <v-btn icon ripple>
              <v-icon >navigate_next</v-icon>
            </v-btn>
          </v-list-tile-action>
        </v-list-tile>
      </v-list>
  </v-container>
</template>

<script>
export default {
  name: 'Config',
  methods: {
    setting() {
      this.$router.push('/setting');
    }
  }
}
</script>

