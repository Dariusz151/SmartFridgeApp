import axios from "axios";

const apiEndpoint = "https://localhost:5001/api";
//const apiEndpoint = "https://smartfridgeapp.com.pl/api/answers";

// export async function postAnswer(data) {
//   return await axios.post(apiEndpoint, data);
// }

export async function getFridges() {
  return await axios.get(apiEndpoint + "/fridges");
}

export async function getFridgeItems(fridgeId, userId) {
  return await axios.get(
    apiEndpoint + "/fridgeItems/" + fridgeId + "/" + userId
  );
}
