<template>
  <v-container style="padding: 0" align-start>
    <v-card class="mb-2">
      <v-list two-line>
        <v-list-tile avatar>
          <v-list-tile-avatar>
            <img src="@/assets/administrator.png">
          </v-list-tile-avatar>

          <v-list-tile-content>
            <v-list-tile-title>Xiaodong</v-list-tile-title>
            <v-list-tile-sub-title>管理员</v-list-tile-sub-title>
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
        <v-list-tile>
          <v-list-tile-avatar>
            <v-icon small>vpn_key</v-icon>
          </v-list-tile-avatar>
          <v-list-tile-content>
            <v-list-tile-title>密码管理</v-list-tile-title>
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
        <v-list-tile>
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
  </v-container>
</template>

<script>
export default {
  
}
</script>

