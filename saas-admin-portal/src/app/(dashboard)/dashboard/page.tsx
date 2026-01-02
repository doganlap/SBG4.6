'use client'

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { 
  Users, Building2, Package, CreditCard, 
  TrendingUp, Activity, Server, AlertCircle 
} from 'lucide-react'
import { StatsCard } from '@/components/dashboard/stats-card'
import { RevenueChart } from '@/components/dashboard/revenue-chart'
import { TenantTable } from '@/components/dashboard/tenant-table'
import { ModuleUsageChart } from '@/components/dashboard/module-usage-chart'
import { SystemHealthCard } from '@/components/dashboard/system-health-card'

export default function DashboardPage() {
  return (
    <div className="space-y-6">
      {/* Page Header */}
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Dashboard</h1>
        <p className="text-muted-foreground">
          Welcome to the ERPNext SaaS Admin Portal
        </p>
      </div>

      {/* Stats Grid */}
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        <StatsCard
          title="Total Tenants"
          value="127"
          description="+12 from last month"
          icon={Building2}
          trend="up"
          trendValue="10.5%"
        />
        <StatsCard
          title="Active Users"
          value="2,847"
          description="+180 from last month"
          icon={Users}
          trend="up"
          trendValue="6.7%"
        />
        <StatsCard
          title="Active Subscriptions"
          value="89"
          description="12 trials ending soon"
          icon={CreditCard}
          trend="neutral"
          trendValue="0%"
        />
        <StatsCard
          title="Monthly Revenue"
          value="$48,250"
          description="+$4,230 from last month"
          icon={TrendingUp}
          trend="up"
          trendValue="9.6%"
        />
      </div>

      {/* Charts Row */}
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-7">
        <Card className="col-span-4">
          <CardHeader>
            <CardTitle>Revenue Overview</CardTitle>
            <CardDescription>Monthly revenue for the last 12 months</CardDescription>
          </CardHeader>
          <CardContent>
            <RevenueChart />
          </CardContent>
        </Card>
        <Card className="col-span-3">
          <CardHeader>
            <CardTitle>Module Usage</CardTitle>
            <CardDescription>Most popular modules by tenant count</CardDescription>
          </CardHeader>
          <CardContent>
            <ModuleUsageChart />
          </CardContent>
        </Card>
      </div>

      {/* System Health and Recent Tenants */}
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-7">
        <Card className="col-span-3">
          <CardHeader>
            <CardTitle>System Health</CardTitle>
            <CardDescription>Current infrastructure status</CardDescription>
          </CardHeader>
          <CardContent>
            <SystemHealthCard />
          </CardContent>
        </Card>
        <Card className="col-span-4">
          <CardHeader>
            <CardTitle>Recent Tenants</CardTitle>
            <CardDescription>Newly registered organizations</CardDescription>
          </CardHeader>
          <CardContent>
            <TenantTable />
          </CardContent>
        </Card>
      </div>
    </div>
  )
}
