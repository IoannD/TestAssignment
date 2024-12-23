# REST API for Uploading and Processing JSON Metadata Files

## Overview

This project implements a .NET 8 RESTful API designed to handle JSON metadata files related to clinical trials. The API
provides functionality to upload, validate, transform, and store JSON data in a SQL database. Additionally, it supports
data retrieval with filtering.

---

## Features

- **File Upload:**
    - Supports `.json` files with size constraints.
    - Validates JSON metadata against a predefined schema.

- **Data Processing:**
    - Transforms and normalizes data using business rules:
        - Calculates trial duration in days.
        - Sets a default `endDate` for ongoing trials.
    - Maps data to a PostgreSQL database using Entity Framework Core.

- **Endpoints:**
    - Retrieve specific records by ID.
    - Filter records using query parameters (e.g., status) and paging.

- **Documentation:**
    - Swagger integration for interactive API documentation for Development environment.

- **Containerization:**
    - Docker support for consistent deployment across environments.
    - Docker Compose allows fast deployment without additional dependencies.

---

## Setup Instructions

1. **Clone the repository:**

2. **Run the API Using Docker Compose:**
   ```bash
   docker compose up
3. **Access the API and Swagger Documentation:**
    - API: Accessible at http://localhost:5555/
    - Swagger Documentation: Available at http://localhost:5555/swagger

