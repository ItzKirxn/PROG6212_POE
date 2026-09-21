# RaceDay - API Endpoint Plan

**Module:** Programming 3  
**Module Code:** PROG6212  
**Assessment:** POE Part 1  
**System:** RaceDay Event Management System

---

## 1. Authentication

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| POST | `/api/auth/register` | Registers a new user account. | Public | `{ fullName, email, password, role, phoneNumber }` | 201 Created, 400 Bad Request, 409 Conflict |
| POST | `/api/auth/login` | Authenticates a user and issues a JWT token. | Public | `{ email, password }` | 200 OK with token, 401 Unauthorized |

### Register

A new participant or organiser can create an account using the registration endpoint. The password will be securely hashed when the API is implemented in Part 2.

### Login

The login endpoint verifies the user's credentials and returns a JWT containing the user's identity and role. The token will be used to protect role-specific endpoints.

---

## 2. User Profile

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | `/api/users/me` | Retrieves the profile of the currently logged-in user. | Authenticated User | None | 200 OK with profile, 401 Unauthorized |
| PUT | `/api/users/me` | Updates the currently logged-in user's profile. | Authenticated User | `{ fullName, phoneNumber }` | 200 OK, 400 Bad Request, 401 Unauthorized |

---

## 3. Events

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | `/api/events` | Retrieves upcoming events. Supports optional filtering by province, event type and date. | Public | None | 200 OK with event list |
| GET | `/api/events/{id}` | Retrieves the details of a specific event, including its categories. | Public | None | 200 OK, 404 Not Found |
| POST | `/api/events` | Creates a new event for the logged-in organiser. | Organiser | `{ eventName, description, eventType, eventDate, startTime, location, province }` | 201 Created, 400 Bad Request, 401 Unauthorized |
| PUT | `/api/events/{id}` | Updates an event owned by the logged-in organiser. | Organiser | `{ eventName, description, eventType, eventDate, startTime, location, province }` | 200 OK, 403 Forbidden, 404 Not Found |
| DELETE | `/api/events/{id}` | Deletes an event and its associated categories and routes, subject to enrolment and result constraints. | Organiser | None | 204 No Content, 403 Forbidden, 404 Not Found |

### Event Filtering

The `GET /api/events` endpoint may support query parameters such as:

```text
/api/events?province=Gauteng
/api/events?eventType=Running
/api/events?date=2027-11-07