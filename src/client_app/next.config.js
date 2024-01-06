/** @type {import('next').NextConfig} */
//
const nextConfig = {
    images: {
      remotePatterns: [
        {
          protocol: "https",
          hostname: "**",
        },
      ],
    },
  };
  
module.exports = {
    ...nextConfig,
    async rewrites() {
        return [
            {
                source: '/asp/api/:path*',
                destination: 'http://localhost:5000/api/:path*',
            }
        ]
    }
  }
