server {
    listen 80;
    server_name stage.iq-s.pro www.stage.iq-s.pro;
    return 301 https://$host$request_uri;
	large_client_header_buffers 4 128k;
}

server {
	listen 443 ssl;
	listen [::]:443 ssl;
	server_name stage.iq-s.pro www.stage.iq-s.pro;
	ssl_certificate /etc/ssl/iq-s.pro/iq-s.pro.crt;
	ssl_certificate_key /etc/ssl/iq-s.pro/iq-s.pro.key;
	large_client_header_buffers 4 128k;

	access_log /var/log/nginx/nginx.web.stage.iq-s.pro.access.log;
	error_log /var/log/nginx/nginx.web.stage.iq-s.pro.error.log;

	add_header X-Content-Type-Options "nosniff";
	add_header 'Access-Control-Allow-Headers' 'token-access,DNT,X-CustomHeader,Keep-Alive,User-Agent,X-Requested-With,If-Modified-Since,Cache-Control,Content-Type' always;

    location / {
		proxy_pass https://localhost:5050;
		proxy_http_version 1.1;
		proxy_set_header Upgrade $http_upgrade;
		proxy_set_header Connection $http_connection;
		proxy_set_header Host $host;
		proxy_cache_bypass $http_upgrade;
			
		if ($request_method = 'OPTIONS') {
			add_header 'Access-Control-Allow-Origin' '*';
			add_header 'Access-Control-Allow-Credentials' 'true';
			add_header 'Access-Control-Allow-Methods' 'GET, DELETE, POST, PUT, PATCH, OPTIONS' always;	 
			add_header 'Access-Control-Max-Age' 1728000;
			add_header 'Content-Type' 'text/plain charset=UTF-8';
			add_header 'Content-Length' 0;
			return 204;
		 }
		 if ($request_method = 'POST') {
			add_header 'Access-Control-Allow-Credentials' 'true';
			add_header 'Access-Control-Allow-Methods' 'GET, DELETE, POST, PUT, PATCH, OPTIONS' always;
		 }
		 if ($request_method = 'PUT') {
			add_header 'Access-Control-Allow-Credentials' 'true';
			add_header 'Access-Control-Allow-Methods' 'GET, DELETE, POST, PUT, PATCH, OPTIONS' always;
		 }
		 if ($request_method = 'PATCH') {
			add_header 'Access-Control-Allow-Credentials' 'true';
			add_header 'Access-Control-Allow-Methods' 'GET, DELETE, POST, PUT, PATCH, OPTIONS' always;
		 }
		 if ($request_method = 'GET') {
			add_header 'Access-Control-Allow-Credentials' 'true' always;
			add_header 'Access-Control-Allow-Methods' 'GET, DELETE, POST, PUT, PATCH, OPTIONS' always;
		}
		 if ($request_method = 'DELETE') {
			add_header 'Access-Control-Allow-Credentials' 'true';
			add_header 'Access-Control-Allow-Methods' 'GET, DELETE, POST, PUT, PATCH, OPTIONS' always;
		}
    }
}