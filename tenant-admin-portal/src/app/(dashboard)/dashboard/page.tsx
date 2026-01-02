'use client'

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Users, Package, Activity, Clock, Settings, FileText, Bell, Shield } from 'lucide-react'
import { Button } from '@/components/ui/button'

const quickActions = [
  { title: 'Manage Users', description: 'Add or remove users', icon: Users, href: '/users' },
  { title: 'Modules', description: 'Configure your modules', icon: Package, href: '/modules' },
  { title: 'Settings', description: 'Organization settings', icon: Settings, href: '/settings' },
  { title: 'Reports', description: 'View usage reports', icon: FileText, href: '/reports' },
]

const stats = [
  { title: 'Total Users', value: '45', change: '+3 this month', icon: Users },
  { title: 'Active Modules', value: '8', change: '2 available', icon: Package },
  { title: 'Storage Used', value: '12.5 GB', change: '37.5 GB remaining', icon: Activity },
  { title: 'API Calls', value: '125,430', change: 'This month', icon: Clock },
]

const recentActivity = [
  { user: 'John Doe', action: 'Added new Sales Order #1234', time: '2 minutes ago' },
  { user: 'Jane Smith', action: 'Updated Customer record', time: '15 minutes ago' },
  { user: 'Mike Johnson', action: 'Generated Invoice #5678', time: '1 hour ago' },
  { user: 'Sarah Wilson', action: 'Created Purchase Order', time: '2 hours ago' },
  { user: 'System', action: 'Daily backup completed', time: '6 hours ago' },
]

export default function TenantDashboard() {
  return (
    <div className="space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Dashboard</h1>
        <p className="text-muted-foreground">
          Welcome to your organization's admin portal
        </p>
      </div>

      {/* Stats Grid */}
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        {stats.map((stat) => (
          <Card key={stat.title}>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">{stat.title}</CardTitle>
              <stat.icon className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{stat.value}</div>
              <p className="text-xs text-muted-foreground">{stat.change}</p>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* Quick Actions */}
      <Card>
        <CardHeader>
          <CardTitle>Quick Actions</CardTitle>
          <CardDescription>Common administrative tasks</CardDescription>
        </CardHeader>
        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
            {quickActions.map((action) => (
              <Button
                key={action.title}
                variant="outline"
                className="h-auto flex-col items-start gap-2 p-4"
              >
                <action.icon className="h-5 w-5" />
                <div className="text-left">
                  <div className="font-medium">{action.title}</div>
                  <div className="text-xs text-muted-foreground">
                    {action.description}
                  </div>
                </div>
              </Button>
            ))}
          </div>
        </CardContent>
      </Card>

      {/* Recent Activity and Alerts */}
      <div className="grid gap-4 md:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>Recent Activity</CardTitle>
            <CardDescription>Latest actions in your organization</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {recentActivity.map((activity, i) => (
                <div key={i} className="flex items-start gap-3">
                  <div className="h-8 w-8 rounded-full bg-muted flex items-center justify-center text-xs font-medium">
                    {activity.user.split(' ').map(n => n[0]).join('')}
                  </div>
                  <div className="flex-1">
                    <p className="text-sm">
                      <span className="font-medium">{activity.user}</span>{' '}
                      {activity.action}
                    </p>
                    <p className="text-xs text-muted-foreground">{activity.time}</p>
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>System Alerts</CardTitle>
            <CardDescription>Important notifications</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <div className="flex items-start gap-3 rounded-lg border p-3">
                <Bell className="h-5 w-5 text-yellow-500" />
                <div>
                  <p className="font-medium text-sm">Subscription Renewal</p>
                  <p className="text-xs text-muted-foreground">
                    Your subscription renews in 15 days
                  </p>
                </div>
              </div>
              <div className="flex items-start gap-3 rounded-lg border p-3">
                <Shield className="h-5 w-5 text-green-500" />
                <div>
                  <p className="font-medium text-sm">Security Update</p>
                  <p className="text-xs text-muted-foreground">
                    Two-factor authentication is enabled
                  </p>
                </div>
              </div>
              <div className="flex items-start gap-3 rounded-lg border p-3">
                <Package className="h-5 w-5 text-blue-500" />
                <div>
                  <p className="font-medium text-sm">New Module Available</p>
                  <p className="text-xs text-muted-foreground">
                    The E-commerce module is now available
                  </p>
                </div>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  )
}
