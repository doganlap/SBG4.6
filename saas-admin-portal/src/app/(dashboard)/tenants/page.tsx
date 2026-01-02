'use client'

import { useState } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { 
  Table, TableBody, TableCell, TableHead, 
  TableHeader, TableRow 
} from '@/components/ui/table'
import { Badge } from '@/components/ui/badge'
import { 
  DropdownMenu, DropdownMenuContent, DropdownMenuItem, 
  DropdownMenuLabel, DropdownMenuSeparator, DropdownMenuTrigger 
} from '@/components/ui/dropdown-menu'
import { 
  Plus, Search, MoreHorizontal, Building2, 
  Users, Package, Settings, Trash2, Eye 
} from 'lucide-react'
import { CreateTenantDialog } from '@/components/tenants/create-tenant-dialog'

const tenants = [
  {
    id: 'TNT001',
    name: 'Acme Corporation',
    domain: 'acme.erp.local',
    plan: 'Enterprise',
    status: 'active',
    users: 45,
    modules: ['Accounting', 'CRM', 'HR', 'Stock'],
    createdAt: '2024-01-15',
    mrr: 2500,
  },
  {
    id: 'TNT002',
    name: 'Global Industries',
    domain: 'global.erp.local',
    plan: 'Professional',
    status: 'active',
    users: 28,
    modules: ['Accounting', 'Selling', 'Buying', 'Manufacturing'],
    createdAt: '2024-02-20',
    mrr: 1500,
  },
  {
    id: 'TNT003',
    name: 'TechStart Inc',
    domain: 'techstart.erp.local',
    plan: 'Starter',
    status: 'trial',
    users: 5,
    modules: ['Accounting', 'CRM'],
    createdAt: '2024-03-01',
    mrr: 0,
  },
  {
    id: 'TNT004',
    name: 'MediCare Hospital',
    domain: 'medicare.erp.local',
    plan: 'Enterprise',
    status: 'active',
    users: 120,
    modules: ['Healthcare', 'Accounting', 'HR', 'Stock'],
    createdAt: '2024-01-05',
    mrr: 4500,
  },
  {
    id: 'TNT005',
    name: 'EduWorld School',
    domain: 'eduworld.erp.local',
    plan: 'Professional',
    status: 'suspended',
    users: 35,
    modules: ['Education', 'Accounting', 'HR'],
    createdAt: '2024-02-10',
    mrr: 0,
  },
]

export default function TenantsPage() {
  const [searchQuery, setSearchQuery] = useState('')
  const [isCreateOpen, setIsCreateOpen] = useState(false)

  const filteredTenants = tenants.filter(tenant =>
    tenant.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
    tenant.domain.toLowerCase().includes(searchQuery.toLowerCase())
  )

  return (
    <div className="space-y-6">
      {/* Page Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Tenants</h1>
          <p className="text-muted-foreground">
            Manage all tenant organizations on the platform
          </p>
        </div>
        <Button onClick={() => setIsCreateOpen(true)}>
          <Plus className="mr-2 h-4 w-4" />
          Create Tenant
        </Button>
      </div>

      {/* Stats Cards */}
      <div className="grid gap-4 md:grid-cols-4">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Total Tenants</CardTitle>
            <Building2 className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{tenants.length}</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Active</CardTitle>
            <Badge variant="default" className="bg-green-500">Active</Badge>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">
              {tenants.filter(t => t.status === 'active').length}
            </div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Trial</CardTitle>
            <Badge variant="secondary">Trial</Badge>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">
              {tenants.filter(t => t.status === 'trial').length}
            </div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Total MRR</CardTitle>
            <span className="text-green-500">$</span>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">
              ${tenants.reduce((sum, t) => sum + t.mrr, 0).toLocaleString()}
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Tenants Table */}
      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <div>
              <CardTitle>All Tenants</CardTitle>
              <CardDescription>A list of all tenant organizations</CardDescription>
            </div>
            <div className="relative w-64">
              <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
              <Input
                placeholder="Search tenants..."
                className="pl-8"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
              />
            </div>
          </div>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Organization</TableHead>
                <TableHead>Domain</TableHead>
                <TableHead>Plan</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Users</TableHead>
                <TableHead>Modules</TableHead>
                <TableHead>MRR</TableHead>
                <TableHead className="text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filteredTenants.map((tenant) => (
                <TableRow key={tenant.id}>
                  <TableCell className="font-medium">{tenant.name}</TableCell>
                  <TableCell>{tenant.domain}</TableCell>
                  <TableCell>
                    <Badge variant="outline">{tenant.plan}</Badge>
                  </TableCell>
                  <TableCell>
                    <Badge
                      variant={
                        tenant.status === 'active' ? 'default' :
                        tenant.status === 'trial' ? 'secondary' : 'destructive'
                      }
                      className={
                        tenant.status === 'active' ? 'bg-green-500' : ''
                      }
                    >
                      {tenant.status}
                    </Badge>
                  </TableCell>
                  <TableCell>{tenant.users}</TableCell>
                  <TableCell>
                    <div className="flex flex-wrap gap-1">
                      {tenant.modules.slice(0, 2).map((mod) => (
                        <Badge key={mod} variant="outline" className="text-xs">
                          {mod}
                        </Badge>
                      ))}
                      {tenant.modules.length > 2 && (
                        <Badge variant="outline" className="text-xs">
                          +{tenant.modules.length - 2}
                        </Badge>
                      )}
                    </div>
                  </TableCell>
                  <TableCell>${tenant.mrr.toLocaleString()}</TableCell>
                  <TableCell className="text-right">
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <Button variant="ghost" className="h-8 w-8 p-0">
                          <MoreHorizontal className="h-4 w-4" />
                        </Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align="end">
                        <DropdownMenuLabel>Actions</DropdownMenuLabel>
                        <DropdownMenuItem>
                          <Eye className="mr-2 h-4 w-4" />
                          View Details
                        </DropdownMenuItem>
                        <DropdownMenuItem>
                          <Settings className="mr-2 h-4 w-4" />
                          Configure
                        </DropdownMenuItem>
                        <DropdownMenuItem>
                          <Package className="mr-2 h-4 w-4" />
                          Manage Modules
                        </DropdownMenuItem>
                        <DropdownMenuItem>
                          <Users className="mr-2 h-4 w-4" />
                          Manage Users
                        </DropdownMenuItem>
                        <DropdownMenuSeparator />
                        <DropdownMenuItem className="text-destructive">
                          <Trash2 className="mr-2 h-4 w-4" />
                          Delete Tenant
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>

      <CreateTenantDialog open={isCreateOpen} onOpenChange={setIsCreateOpen} />
    </div>
  )
}
