import axios from "axios";

const apiEndpoint = "https://localhost:5001/api";

export async function getFridges() {
  return await axios.get(apiEndpoint + "/fridges");
}
