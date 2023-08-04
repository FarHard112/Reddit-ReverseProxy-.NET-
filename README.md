# Reddit Proxy Server

## Introduction
This project serves as a proxy server for Reddit.com. It fetches content from Reddit, modifies specific text within the HTML, and sends the modified content to the client. The main features include appending a "™" symbol to six-letter words and ensuring all internal navigation links point to the proxy server.

## Features
- **URL Validation:** Checks if the provided URL is a valid Reddit URL.
- **User-Agent Specification:** Adds a User-Agent string to request headers for compatibility.
- **Content Modification:** Appends the "™" symbol to six-letter words within text content, excluding certain HTML elements like buttons, inputs, and scripts to maintain site functionality.
- **Internal Navigation Handling:** Modifies internal navigation links to ensure that all navigation is handled through the proxy server.

## How to Use
### Clone the Repository
Clone this repository to your local machine.

### Install Dependencies
Make sure to have the required dependencies, such as the appropriate .NET SDK (.Net 6.0).


### Run the Project
Start the server, and it will listen for requests to proxy Reddit content.

### Navigate through the Proxy
Use the proxy URL with the appropriate query parameters to view modified Reddit content.
For viewing https://reddit.com/r/popular , just type https://localhost:yourport/r/popular 

## Docker Deployment
### Building the Docker Image
#### Navigate to the Directory
Open a terminal and navigate to the directory containing the Dockerfile.

#### Build the Image
Use the following bash command to build the Docker image:
```bash
docker build -t reddit-proxy .
```

For running:

```bash
docker run -p 80:80 -p 443:443 reddit-proxy
```


