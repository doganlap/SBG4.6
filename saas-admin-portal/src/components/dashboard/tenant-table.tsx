'use client'

import { Badge } from '@/components/ui/badge'
import { Avatar, AvatarFallback } from '@/components/ui/avatar'

const recentTenants = [
  {
    id: 1,
    name: 'TechStart Inc',
    email: 'admin@techstart.com',
    plan: 'Starter',
    status: 'trial',
    createdAt: '2 hours ago',
  },
  {
    id: 2,
    name: 'Global Industries',
    email: 'admin@global-ind.com',
    plan: 'Professional',
    status: 'active',
    createdAt: '1 day ago',
  },
  {
    id: 3,
    name: 'MediCare Hospital',
    email: 'it@medicare.com',
    plan: 'Enterprise',
    status: 'active',
    createdAt: '2 days ago',
  },
  {
    id: 4,
    name: 'EduWorld School',
    email: 'admin@eduworld.edu',
    plan: 'Professional',
    status: 'pending',
    createdAt: '3 days ago',
  },
  {
    id: 5,
    name: 'RetailMax',
    email: 'ops@retailmax.com',
    plan: 'Enterprise',
    status: 'active',
    createdAt: '5 days ago',
  },
]

export function TenantTable() {
  return (
    <div className="space-y-4">
      {recentTenants.map((tenant) => (
        <div
          key={tenant.id}
          className="flex items-center justify-between rounded-lg border p-3"
        >
          <div className="flex items-center gap-3">
            <Avatar className="h-10 w-10">
              <AvatarFallback>
                {tenant.name.split(' ').map(n => n[0]).join('').slice(0, 2)}
              </AvatarFallback>
            </Avatar>
            <div>
              <p className="font-medium">{tenant.name}</p>
              <p className="text-sm text-muted-foreground">{tenant.email}</p>
            </div>
          </div>
          <div className="flex items-center gap-4">
            <Badge variant="outline">{tenant.plan}</Badge>
            <Badge
              variant={
                tenant.status === 'active' ? 'default' :
                tenant.status === 'trial' ? 'secondary' : 'outline'
              }
              className={tenant.status === 'active' ? 'bg-green-500' : ''}
            >
              {tenant.status}
            </Badge>
            <span className="text-sm text-muted-foreground w-20 text-right">
              {tenant.createdAt}
            </span>
          </div>
        </div>
      ))}
    </div>
  )
}
