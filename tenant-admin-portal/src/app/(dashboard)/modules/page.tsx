'use client'

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Switch } from '@/components/ui/switch'
import { 
  Calculator, Users, ShoppingCart, Package, Factory, 
  ClipboardList, Headphones, Globe, CreditCard, Stethoscope,
  GraduationCap, Truck, Settings
} from 'lucide-react'

const modules = [
  {
    id: 'accounting',
    name: 'Accounting',
    description: 'Financial management, ledgers, and reports',
    icon: Calculator,
    enabled: true,
    included: true,
  },
  {
    id: 'crm',
    name: 'CRM',
    description: 'Customer relationship management',
    icon: Users,
    enabled: true,
    included: true,
  },
  {
    id: 'selling',
    name: 'Selling',
    description: 'Sales orders, quotations, invoices',
    icon: ShoppingCart,
    enabled: true,
    included: true,
  },
  {
    id: 'buying',
    name: 'Buying',
    description: 'Purchase orders, supplier management',
    icon: Truck,
    enabled: true,
    included: true,
  },
  {
    id: 'stock',
    name: 'Stock',
    description: 'Inventory management, warehouses',
    icon: Package,
    enabled: true,
    included: true,
  },
  {
    id: 'manufacturing',
    name: 'Manufacturing',
    description: 'Production planning, BOM, work orders',
    icon: Factory,
    enabled: false,
    included: true,
  },
  {
    id: 'hr',
    name: 'HRMS',
    description: 'Employee management, payroll, attendance',
    icon: ClipboardList,
    enabled: true,
    included: true,
  },
  {
    id: 'support',
    name: 'Support',
    description: 'Helpdesk, tickets, SLA',
    icon: Headphones,
    enabled: true,
    included: true,
  },
  {
    id: 'website',
    name: 'Website',
    description: 'Web pages, blog, portal',
    icon: Globe,
    enabled: false,
    included: false,
  },
  {
    id: 'ecommerce',
    name: 'E-commerce',
    description: 'Online store, shopping cart',
    icon: CreditCard,
    enabled: false,
    included: false,
  },
  {
    id: 'healthcare',
    name: 'Healthcare',
    description: 'Patients, appointments, lab tests',
    icon: Stethoscope,
    enabled: false,
    included: false,
  },
  {
    id: 'education',
    name: 'Education',
    description: 'Schools, courses, students',
    icon: GraduationCap,
    enabled: false,
    included: false,
  },
]

export default function ModulesPage() {
  const enabledCount = modules.filter(m => m.enabled).length
  const includedCount = modules.filter(m => m.included).length

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Modules</h1>
          <p className="text-muted-foreground">
            Configure which ERPNext modules are available
          </p>
        </div>
        <Button variant="outline">
          <Settings className="mr-2 h-4 w-4" />
          Module Settings
        </Button>
      </div>

      {/* Stats */}
      <div className="grid gap-4 md:grid-cols-3">
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium">Active Modules</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{enabledCount}</div>
            <p className="text-xs text-muted-foreground">Currently enabled</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium">Included in Plan</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{includedCount}</div>
            <p className="text-xs text-muted-foreground">Available to enable</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium">Add-on Modules</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{modules.length - includedCount}</div>
            <p className="text-xs text-muted-foreground">Available for purchase</p>
          </CardContent>
        </Card>
      </div>

      {/* Modules Grid */}
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
        {modules.map((module) => (
          <Card key={module.id} className={!module.included ? 'opacity-60' : ''}>
            <CardHeader>
              <div className="flex items-start justify-between">
                <div className="flex items-center gap-3">
                  <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10">
                    <module.icon className="h-5 w-5 text-primary" />
                  </div>
                  <div>
                    <CardTitle className="text-lg">{module.name}</CardTitle>
                    <div className="flex gap-2 mt-1">
                      {module.included ? (
                        <Badge variant="secondary" className="text-xs">
                          Included
                        </Badge>
                      ) : (
                        <Badge variant="outline" className="text-xs">
                          Add-on
                        </Badge>
                      )}
                    </div>
                  </div>
                </div>
                {module.included && (
                  <Switch checked={module.enabled} />
                )}
              </div>
            </CardHeader>
            <CardContent>
              <CardDescription>{module.description}</CardDescription>
              {!module.included && (
                <Button variant="outline" size="sm" className="mt-4 w-full">
                  Upgrade to Enable
                </Button>
              )}
            </CardContent>
          </Card>
        ))}
      </div>
    </div>
  )
}
