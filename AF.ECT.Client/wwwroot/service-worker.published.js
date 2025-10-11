self.importScripts('./service-worker-assets.js');

// Install event - cache static assets
self.addEventListener('install', event => {
  event.waitUntil(
    caches.open('ect-cache-v1').then(cache => {
      return cache.addAll([
        '/',
        '/index.html',
        '/manifest.json',
        '/css/app.css',
        '/favicon.png',
        '/icon-192.png'
      ]);
    })
  );
});

// Fetch event - serve from cache if offline
self.addEventListener('fetch', event => {
  event.respondWith(
    caches.match(event.request).then(response => {
      return response || fetch(event.request);
    })
  );
});