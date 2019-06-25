import axios from "axios";
import storage from "./storage";

export default {
  login: function(username, password) {
    return axios.post(`${storage.getServiceUrl()}/api/users/login`, {
      loginID: username,
      password: password
    }).then(response => {
      return response.data;
    });
  },
  changeUserPass: function(userId, newpass) {
    return axios.patch(`${storage.getServiceUrl()}/api/users/${userId}`, {
      password: newpass
    }).then(response => {
      return response.data;
    });
  },
  getStations: function(city, name, id) {
    let url = `${storage.getServiceUrl()}/api/stations?`;
    if (id) {
      url = `${url}id=${id}`;
    } else if (name) {
      url = `${url}name=${name}`;
    } else if (city) {
      url = `${url}city=${city}`;
    }

    return axios.get(url, {
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        "Access-Control-Allow-Origin": "*",
      }, 
      data: {}
    })
    .then(response => {
      return response.data;
    });
  },
  getStationCoordinators(stationId) {
    return axios.get(`${storage.getServiceUrl()}/api/stations/${stationId}/coordinators`,{
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        "Access-Control-Allow-Origin": "*",
      }, 
      data: {}
    }).then(response => {
      return response.data;
    });
  },
  getStationDataLatest(stationId, seqId) {
    return axios.get(`${storage.getServiceUrl()}/api/stationdata/latest?stationid=${stationId}&seqid=${seqId}`, {
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        "Access-Control-Allow-Origin": "*",
      }, 
      data: {}
    }).then(response => {
      return response.data;
    });
  },
  getStationDataByDay(stationId, seqId, date) {
    let start = `${date} 00:00:00`;
    let end = `${date} 23:59:59`;
    return axios.get(
      `${storage.getServiceUrl()}/api/stationdata?stationid=${stationId}&seqid=${seqId}&start=${start}&end=${end}&count=100`, {
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          "Access-Control-Allow-Origin": "*",
        }, 
        data: {}
      }).then(response => {
        return response.data;
      });
  },
  getStationAvgDataByDate(stationId, seqId, startdate, enddate) {
    let start = `${startdate} 00:00:00`;
    let end = `${enddate} 23:59:59`;
    return axios.get(
      `${storage.getServiceUrl()}/api/stationdata/avg?stationid=${stationId}&seqid=${seqId}&start=${start}&end=${end}`, {
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          "Access-Control-Allow-Origin": "*",
        }, 
        data: {}
      }).then(response => {
        return response.data;
      }); 
  },
  getCitiesList() {
    return axios.get(`${storage.getServiceUrl()}/api/cities`, {
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        "Access-Control-Allow-Origin": "*",
      }, 
      data: {}
    }).then(response => {
      return response.data;
    });
  },
  getStationComments(stationId) {
    return axios.get(`${storage.getServiceUrl()}/api/stations/${stationId}/comments`, {
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        "Access-Control-Allow-Origin": "*",
      }, 
      data: {}
    }).then(response => {
      return response.data;
    });
  },
  addComment(stationId, comment) {
    return axios.post(`${storage.getServiceUrl()}/api/stations/${stationId}/comments`, {
      comment: comment
    }).then(response => {
      return response.data;
    });
  },
  saveStation(stationId, station) {
    return axios.patch(`${storage.getServiceUrl()}/api/stations/${stationId}`, station)
    .then(response => {
      return response.data;
    });
  },
  addStation(station) {
    return axios.post(`${storage.getServiceUrl()}/api/stations`, station)
    .then(response => {
      return response.data;
    });
  },
  loadCoordinators(station) {
    return axios.get(`${storage.getServiceUrl()}/api/stations/${station.id}/coordinators`, {
      headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      "Access-Control-Allow-Origin": "*",
      }, 
      data: {}
    }).then(response => {
      return response.data;
    });
  },
  saveCoordinator(stationid, seqid, coor) {
    return axios.post(`${storage.getServiceUrl()}/api/stations/${stationid}/coordinators/${seqid}`, coor)
    .then(response => {
      return response.data;
    });
  }
}