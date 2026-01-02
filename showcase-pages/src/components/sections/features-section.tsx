'use client'

import { Badge } from '@/components/ui/badge'
import { 
  Zap, Shield, Globe, Layers, Cloud, Lock,
  Smartphone, RefreshCw, Users, BarChart3
} from 'lucide-react'

const features = [
  {
    icon: Cloud,
    title: 'Cloud Native',
    description: 'Fully containerized and scalable architecture. Deploy anywhere - cloud, hybrid, or on-premise.',
  },
  {
    icon: Shield,
    title: 'Enterprise Security',
    description: 'SOC 2 compliant with end-to-end encryption, SSO, and role-based access control.',
  },
  {
    icon: Globe,
    title: 'Multi-tenant',
    description: 'Each customer gets isolated data and customizable modules with dedicated resources.',
  },
  {
    icon: Layers,
    title: 'Modular Design',
    description: 'Enable only the modules you need. Add more as your business grows.',
  },
  {
    icon: Zap,
    title: 'High Performance',
    description: 'Optimized for speed with Redis caching, background workers, and CDN delivery.',
  },
  {
    icon: Lock,
    title: 'Data Privacy',
    description: 'GDPR compliant with data residency options and automated backups.',
  },
  {
    icon: Smartphone,
    title: 'Mobile Ready',
    description: 'Responsive design works on any device. Native mobile apps coming soon.',
  },
  {
    icon: RefreshCw,
    title: 'Auto Updates',
    description: 'Automatic updates with zero downtime. Always on the latest version.',
  },
  {
    icon: Users,
    title: 'Collaboration',
    description: 'Real-time collaboration with comments, mentions, and activity feeds.',
  },
  {
    icon: BarChart3,
    title: 'Analytics',
    description: 'Built-in business intelligence with custom dashboards and reports.',
  },
]

export function FeaturesSection() {
  return (
    <section id="features" className="py-24">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Section Header */}
        <div className="text-center mb-16">
          <Badge variant="outline" className="mb-4">Features</Badge>
          <h2 className="text-4xl md:text-5xl font-bold mb-6">
            Built for Modern Business
          </h2>
          <p className="text-xl text-muted-foreground max-w-2xl mx-auto">
            Enterprise-grade features designed for scalability, security, 
            and seamless user experience.
          </p>
        </div>

        {/* Features Grid */}
        <div className="grid gap-8 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-5">
          {features.map((feature) => (
            <div 
              key={feature.title}
              className="group p-6 rounded-2xl border bg-card hover:border-primary/50 hover:shadow-lg transition-all duration-300"
            >
              <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-primary/10 group-hover:bg-primary/20 transition-colors mb-4">
                <feature.icon className="h-6 w-6 text-primary" />
              </div>
              <h3 className="font-semibold text-lg mb-2">{feature.title}</h3>
              <p className="text-sm text-muted-foreground">{feature.description}</p>
            </div>
          ))}
        </div>
      </div>
    </section>
  )
}
