# GraphQL API Documentation

## Endpoint
- **URL**: `https://localhost:7053/graphql` (HTTPS) or `http://localhost:5010/graphql` (HTTP)
- **Method**: `POST`
- **Content-Type**: `application/json`

## Queries

### Get All Stations
```json
{
  "query": "query { getStations { stationTriNmid stationTriNmcode stationTriNmname address city province capacity currentAvailable owner contactPhone contactEmail isActive imageUrl } }"
}
```

### Get Station By ID
```json
{
  "query": "query($id: Int!) { getStationById(id: $id) { stationTriNmid stationTriNmcode stationTriNmname address city province capacity currentAvailable owner contactPhone contactEmail isActive imageUrl } }",
  "variables": { "id": 1 }
}
```

### Search Stations
```json
{
  "query": "query($name: String, $location: String, $isActive: Boolean) { searchStations(name: $name, location: $location, isActive: $isActive) { stationTriNmid stationTriNmcode stationTriNmname address city province capacity currentAvailable owner isActive } }",
  "variables": { "name": null, "location": null, "isActive": true }
}
```

### Get Chargers
```json
{
  "query": "query { getChargers { chargerTriNmid stationTriNmid chargerTriNmtype isAvailable imageUrl } }"
}
```

### Get Charger By ID
```json
{
  "query": "query($id: Int!) { getChargerById(id: $id) { chargerTriNmid stationTriNmid chargerTriNmtype isAvailable imageUrl } }",
  "variables": { "id": 1 }
}
```

## Mutations

### Create Station
```json
{
  "query": "mutation CreateStation($input: CreateStationInput!) { createStation(input: $input) { stationTriNmid stationTriNmcode stationTriNmname address city province capacity currentAvailable owner contactPhone contactEmail isActive imageUrl } }",
  "variables": {
    "input": {
      "stationCode": "ST001",
      "stationName": "New Station",
      "address": "123 Main St",
      "city": "HCMC",
      "province": "HCMC",
      "latitude": 10.762622,
      "longitude": 106.660172,
      "capacity": 10,
      "owner": "Owner Name",
      "contactPhone": "0123456789",
      "contactEmail": "owner@example.com",
      "description": "Description",
      "imageURL": null
    }
  }
}
```

### Update Station
```json
{
  "query": "mutation UpdateStation($input: UpdateStationInput!) { updateStation(input: $input) { stationTriNmid stationTriNmcode stationTriNmname address city province capacity currentAvailable owner contactPhone contactEmail isActive imageUrl } }",
  "variables": {
    "input": {
      "stationId": 1,
      "stationName": "Updated Station",
      "capacity": 15,
      "isActive": true
    }
  }
}
```

### Delete Station
```json
{
  "query": "mutation DeleteStation($id: Int!) { deleteStation(id: $id) }",
  "variables": { "id": 1 }
}
```

### Login
```json
{
  "query": "mutation Login($input: LoginInput!) { login(input: $input) { token error } }",
  "variables": {
    "input": {
      "email": "user@example.com",
      "password": "password123"
    }
  }
}
```

### Google Login
```json
{
  "query": "mutation GoogleLogin($credential: String!) { googleLogin(credential: $credential) { token error } }",
  "variables": {
    "credential": "google_jwt_token_here"
  }
}
```

## Authentication
Add header for authenticated requests:
```
Authorization: Bearer <your_jwt_token>
```

## Introspection
```json
{
  "query": "query { __schema { queryType { fields { name args { name type { name } } } } mutationType { fields { name args { name type { name } } } } } }"
}
```
