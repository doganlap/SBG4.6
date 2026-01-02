'use client'

import { cn } from '@/lib/utils'
import { CheckCircle2, AlertCircle, XCircle, Activity } from 'lucide-react'
import { Progress } from '@/components/ui/progress'

const services = [
  { name: 'ERPNext Backend', status: 'healthy', latency: 45, uptime: 99.99 },
  { name: 'MariaDB Cluster', status: 'healthy', latency: 12, uptime: 99.98 },
  { name: 'Redis Cache', status: 'healthy', latency: 3, uptime: 100 },
  { name: 'Nginx Proxy', status: 'healthy', latency: 8, uptime: 99.99 },
  { name: 'Background Workers', status: 'warning', latency: 125, uptime: 99.5 },
]

const resourceUsage = [
  { name: 'CPU Usage', value: 42, max: 100 },
  { name: 'Memory', value: 68, max: 100 },
  { name: 'Storage', value: 35, max: 100 },
  { name: 'Bandwidth', value: 28, max: 100 },
]

export function SystemHealthCard() {
  return (
    <div className="space-y-6">
      {/* Services Status */}
      <div className="space-y-3">
        <h4 className="text-sm font-medium">Services Status</h4>
        {services.map((service) => (
          <div key={service.name} className="flex items-center justify-between">
            <div className="flex items-center gap-2">
              {service.status === 'healthy' ? (
                <CheckCircle2 className="h-4 w-4 text-green-500" />
              ) : service.status === 'warning' ? (
                <AlertCircle className="h-4 w-4 text-yellow-500" />
              ) : (
                <XCircle className="h-4 w-4 text-red-500" />
              )}
              <span className="text-sm">{service.name}</span>
            </div>
            <div className="flex items-center gap-4">
              <span className="text-xs text-muted-foreground">
                {service.latency}ms
              </span>
              <span className="text-xs text-muted-foreground">
                {service.uptime}%
              </span>
            </div>
          </div>
        ))}
      </div>

      {/* Resource Usage */}
      <div className="space-y-3">
        <h4 className="text-sm font-medium">Resource Usage</h4>
        {resourceUsage.map((resource) => (
          <div key={resource.name} className="space-y-1">
            <div className="flex items-center justify-between text-sm">
              <span>{resource.name}</span>
              <span className="text-muted-foreground">{resource.value}%</span>
            </div>
            <Progress
              value={resource.value}
              className={cn(
                'h-2',
                resource.value > 80 && 'bg-red-100',
                resource.value > 60 && resource.value <= 80 && 'bg-yellow-100'
              )}
            />
          </div>
        ))}
      </div>
    </div>
  )
}
