<template>
  <v-container fluid>
    <v-layout class="mt-3 mx-5" column>
      <v-flex xs12>
        <v-toolbar flat class="transparent">
          <v-toolbar-title>用户管理</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-btn icon @click="refresh">
            <v-icon>refresh</v-icon>
          </v-btn>
          <v-btn icon @click="addUser">
            <v-icon>add_circle_outline</v-icon>
          </v-btn>          
        </v-toolbar>
      </v-flex>
      <v-flex xs12>
        <v-data-table
          :headers="headers"
          :items="users"
          class="elevation-1"
        >
          <template slot="items" slot-scope="props">
            <td>{{ props.item.loginID }}</td>
            <td class="text-xs-right">{{ props.item.name }}</td>
            <td class="text-xs-right">{{ props.item.isAdmin }}</td>
            <td class="text-xs-right">{{ props.item.email }}</td>
            <td class="text-xs-right">{{ props.item.mobilePhone }}</td>
            <td class="text-xs-right">{{ tolocaltime(props.item.createdAt) }}</td>
            <td class="text-xs-right">{{ tolocaltime(props.item.lastLoginAt) }}</td>            
            <td class="justify-center layout px-0">
              <v-icon
                small
                class="mr-3"
                @click="editUser(props.item)"
              >
                edit
              </v-icon>
              <v-icon
                small
                @click="changepass(props.item)"
                class="mr-3"
              >
                vpn_key
              </v-icon>
              <v-icon
                small
                @click="deleteUser(props.item)"
                class="mr-3"
              >
                remove_circle_outline
              </v-icon>
            </td>
          </template>
        </v-data-table>
      </v-flex>
    </v-layout>

    <v-dialog v-model="showEditUser" persistent max-width="450">
      <v-form ref="userform" v-model="isValid">
        <v-card>
          <v-list dense class="px-3 pt-3">
            <v-list-tile class="mx-3 mt-4">
              <v-layout>
                <v-flex xs12>
                  <v-text-field
                    v-model="user.loginID"
                    :rules="[rules.required]"
                    type="text"
                    label="登录ID"
                    class="caption"
                  ></v-text-field>
                </v-flex>
              </v-layout>
            </v-list-tile>
            <v-list-tile class="mx-3 mt-4">
              <v-layout>
                <v-flex xs12>
                  <v-text-field
                    v-model="user.name"
                    :rules="[rules.required]"
                    type="text"
                    label="姓名"
                    class="caption"
                  ></v-text-field>
                </v-flex>
              </v-layout>
            </v-list-tile>
            <v-list-tile class="mx-3 mt-4" v-if="isAddUser">
              <v-layout>
                <v-flex xs12>
                  <v-text-field
                    v-model="user.password"
                    :rules="[rules.required]"
                    type="password"
                    label="密码"
                    class="caption"
                  ></v-text-field>
                </v-flex>
              </v-layout>
            </v-list-tile>
            <v-list-tile class="mx-3 mt-4">
              <v-layout>
                <v-flex xs12>
                  <v-text-field
                    v-model="user.title"
                    type="text"
                    label="职位"
                    class="caption"
                  ></v-text-field>
                </v-flex>
              </v-layout>
            </v-list-tile>
            <v-list-tile class="mx-3 mt-4">
              <v-layout>
                <v-flex xs12>
                  <v-text-field
                    v-model="user.email"
                    :rules="[rules.email]"
                    type="text"
                    label="Email"
                    class="caption"
                  ></v-text-field>
                </v-flex>
              </v-layout>
            </v-list-tile>
            <v-list-tile class="mx-3 mt-4">
              <v-layout>
                <v-flex xs12>
                  <v-text-field
                    v-model="user.mobilePhone"
                    type="text"
                    label="手机"
                    class="caption"
                  ></v-text-field>
                </v-flex>
              </v-layout>
            </v-list-tile>
            <v-list-tile class="mx-3 mt-4">
              <v-layout>
                <v-flex xs12>
                  <v-checkbox
                    v-model="user.isAdmin"
                    label="Is Admin"
                    class="caption"
                  ></v-checkbox>
                </v-flex>
              </v-layout>
            </v-list-tile>

          </v-list>
          <v-card-actions class="mt-3 px-3">
            <v-spacer></v-spacer>
            <v-btn color="gray darken-1" flat @click="showEditUser = false">取消</v-btn>
            <v-btn color="primary darken-1" flat @click="saveUser">保存</v-btn>
          </v-card-actions>
        </v-card>
      </v-form>
    </v-dialog>

    <v-dialog v-model="showChangePass" persistent max-width="450">
      <v-form ref="passform" v-model="isValid">
        <v-card>
          <v-list dense class="px-3 pt-3">            
            <v-list-tile class="mx-3 mt-4">
              <v-layout>
                <v-flex xs12>
                  <v-text-field
                    v-model="newpass"
                    :rules="[rules.required]"
                    type="password"
                    label="新密码"
                    class="caption"
                  ></v-text-field>
                </v-flex>
              </v-layout>
            </v-list-tile>
            <v-list-tile class="mx-3 mt-4">
              <v-layout>
                <v-flex xs12>
                  <v-text-field
                    v-model="veripass"
                    :rules="[rules.required, rules.passMatch]"
                    type="password"
                    label="确认密码"
                    class="caption"
                  ></v-text-field>
                </v-flex>
              </v-layout>
            </v-list-tile>
          </v-list>
          <v-card-actions class="mt-3 px-3">
            <v-spacer></v-spacer>
            <v-btn color="gray darken-1" flat @click="showChangePass = false">取消</v-btn>
            <v-btn color="primary darken-1" flat @click="savePass">保存</v-btn>
          </v-card-actions>
        </v-card>
      </v-form>
    </v-dialog>
  </v-container>
</template>

<script>
import axios from 'axios';

export default {
  name: 'Users',
  data() {
    return {
      headers: [
        {text: '登录ID', value: 'loginID'} , 
        {text: '姓名', value: 'name', align: 'right'},
        {text: 'IsAdmin', value: 'isAdmin', align: 'right'},
        {text: 'Email', value: 'email', align: 'right'},
        {text: '手机', value: 'mobilePhone', align: 'right'}, 
        {text: '创建时间', value: 'createdAt', align: 'right'},
        {text: '最后登录时间', value: 'lastLoginAt', align: 'right'},
        {text: '操作', value: '', align: 'center'}
      ],
      users: [],
      showEditUser: false,
      user: {},
      isAddUser: true,
      rules: {
        required: value => !!value || "必填",
        email: v => !v || /.+@.+/.test(v) || "Email格式不正确",
        passMatch: () =>
          this.newpass === this.veripass || "密码不一致",
      },
      newpass: null,
      veripass: null,
      showChangePass: false
    }
  },
  methods: {
    apiurl() {
      let setting = JSON.parse(this.$ls.get('bsm_setting'));
      return `http://${setting.host}:${setting.apiPort}/api`;
    },
    getAllUsers() {
      this.users = [];
      this.$util.showLoading();
      axios.get(this.apiurl() + '/users', {
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          "Access-Control-Allow-Origin": "*",
        }, 
        data: {}
      })
      .then(result => {
        this.$util.hideLoading();
        this.users = result.data;
      }).catch(err => {
        this.$util.hideLoading();
        this.$util.error(`${err.response.data}`);
      });
    },
    refresh() {
      this.getAllUsers();
    },
    addUser() {
      this.isAddUser = true;
      this.user = {};
      this.$refs.userform.resetValidation();
      this.showEditUser = true;
    },
    editUser(user) {
      this.isAddUser = false;
      this.user = user;
      this.$refs.userform.resetValidation();
      this.showEditUser = true;
    },
    changepass(user) {
      this.user = user;
      this.newpass = null;
      this.veripass = null;
      this.$refs.passform.resetValidation();
      this.showChangePass = true;
    },
    deleteUser(user) {
      if (confirm(`确定要删除用户 ${user.name} 吗？`)) {
        this.$util.showLoading();
        axios.delete(this.apiurl() + '/users/' + user.id, {
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Access-Control-Allow-Origin": "*",
          }, 
          data: {}
        })
        .then(() => {
          this.$util.hideLoading();
          this.$util.info('删除用户成功');
          this.getAllUsers();
        }).catch(err => {
          this.$util.hideLoading();
          this.$util.error(`${err.response.data}`);
        });
      }      
    },
    saveUser() {
      if (this.$refs.userform.validate()) {
        if (this.isAddUser) {
          this.$util.showLoading();
          axios.post(this.apiurl() + '/users', this.user)
          .then(() => {
            this.$util.hideLoading();
            this.showEditUser = false;
            this.$util.info('添加用户成功');
            this.getAllUsers();
          }).catch(err => {
            this.$util.hideLoading();
            this.$util.error(`${err.response.data}`);
          })
        } else {
          this.$util.showLoading();
          axios.patch(this.apiurl() + '/users/' + this.user.id, this.user)
          .then(() => {
            this.$util.hideLoading();
            this.showEditUser = false;
            this.$util.info('更新用户成功');
            this.getAllUsers();
          }).catch(err => {
            this.$util.hideLoading();
            this.$util.error(`${err.response.data}`);
          })
        }
      }
    },
    savePass() {
      if (this.$refs.passform.validate()) {
          this.user.password = this.newpass;

          this.$util.showLoading();
          axios.patch(this.apiurl() + '/users/' + this.user.id, this.user)
          .then(() => {
            this.$util.hideLoading();
            this.showChangePass = false;
            this.$util.info('更新用户密码成功');
            this.getAllUsers();
          }).catch(err => {
            this.$util.hideLoading();
            this.$util.error(`${err.response.data}`);
          });
      }
    },
    tolocaltime(time) {
      let dt = new Date(time);
      return dt.toLocaleString('en-US', { timezone: 'Asia/Shanghai'});
    }
  },
  mounted() {
    this.getAllUsers();
  }
}
</script>

