'use client'

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { 
  CreditCard, ExternalLink, Package, Users, 
  HardDrive, Activity, FileText, Settings
} from 'lucide-react'
import Link from 'next/link'

const subscription = {
  plan: 'Professional',
  status: 'active',
  billingCycle: 'Monthly',
  nextBilling: 'January 15, 2026',
  amount: '$1,500/month',
  users: { used: 28, limit: 50 },
  storage: { used: 12.5, limit: 50 },
  modules: 8,
}

const quickLinks = [
  { title: 'Access ERPNext', description: 'Go to your ERPNext instance', icon: ExternalLink, href: 'https://acme.erp.local' },
  { title: 'Manage Users', description: 'Add or remove users', icon: Users, href: '/users' },
  { title: 'Billing & Invoices', description: 'View payment history', icon: CreditCard, href: '/billing' },
  { title: 'Support', description: 'Get help from our team', icon: FileText, href: '/support' },
]

const recentInvoices = [
  { id: 'INV-2024-012', date: 'Dec 15, 2024', amount: '$1,500.00', status: 'paid' },
  { id: 'INV-2024-011', date: 'Nov 15, 2024', amount: '$1,500.00', status: 'paid' },
  { id: 'INV-2024-010', date: 'Oct 15, 2024', amount: '$1,500.00', status: 'paid' },
]

export default function CustomerDashboard() {
  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Welcome back, Acme Corp</h1>
          <p className="text-muted-foreground">
            Manage your subscription and ERPNext access
          </p>
        </div>
        <Button asChild>
          <Link href="https://acme.erp.local" target="_blank">
            <ExternalLink className="mr-2 h-4 w-4" />
            Open ERPNext
          </Link>
        </Button>
      </div>

      {/* Subscription Overview */}
      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <div>
              <CardTitle>Subscription Overview</CardTitle>
              <CardDescription>Your current plan and usage</CardDescription>
            </div>
            <Badge className="bg-green-500">{subscription.status}</Badge>
          </div>
        </CardHeader>
        <CardContent>
          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-4">
            <div className="space-y-2">
              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                <Package className="h-4 w-4" />
                Current Plan
              </div>
              <div className="text-2xl font-bold">{subscription.plan}</div>
              <div className="text-sm text-muted-foreground">
                {subscription.billingCycle} billing
              </div>
            </div>
            <div className="space-y-2">
              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                <CreditCard className="h-4 w-4" />
                Next Billing
              </div>
              <div className="text-2xl font-bold">{subscription.amount}</div>
              <div className="text-sm text-muted-foreground">
                {subscription.nextBilling}
              </div>
            </div>
            <div className="space-y-2">
              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                <Users className="h-4 w-4" />
                Users
              </div>
              <div className="text-2xl font-bold">
                {subscription.users.used}/{subscription.users.limit}
              </div>
              <div className="w-full bg-muted rounded-full h-2">
                <div 
                  className="bg-primary h-2 rounded-full" 
                  style={{ width: `${(subscription.users.used / subscription.users.limit) * 100}%` }}
                />
              </div>
            </div>
            <div className="space-y-2">
              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                <HardDrive className="h-4 w-4" />
                Storage
              </div>
              <div className="text-2xl font-bold">
                {subscription.storage.used} GB
              </div>
              <div className="w-full bg-muted rounded-full h-2">
                <div 
                  className="bg-primary h-2 rounded-full" 
                  style={{ width: `${(subscription.storage.used / subscription.storage.limit) * 100}%` }}
                />
              </div>
            </div>
          </div>
          <div className="mt-6 flex gap-4">
            <Button variant="outline" asChild>
              <Link href="/subscription">Manage Subscription</Link>
            </Button>
            <Button variant="outline" asChild>
              <Link href="/subscription/upgrade">Upgrade Plan</Link>
            </Button>
          </div>
        </CardContent>
      </Card>

      {/* Quick Links */}
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        {quickLinks.map((link) => (
          <Card key={link.title} className="hover:bg-muted/50 transition-colors cursor-pointer">
            <Link href={link.href}>
              <CardHeader>
                <div className="flex items-center gap-3">
                  <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-primary/10">
                    <link.icon className="h-5 w-5 text-primary" />
                  </div>
                  <div>
                    <CardTitle className="text-base">{link.title}</CardTitle>
                    <CardDescription className="text-xs">{link.description}</CardDescription>
                  </div>
                </div>
              </CardHeader>
            </Link>
          </Card>
        ))}
      </div>

      {/* Recent Activity and Invoices */}
      <div className="grid gap-4 md:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>System Status</CardTitle>
            <CardDescription>Your ERPNext instance health</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <div className="h-3 w-3 rounded-full bg-green-500" />
                  <span>ERPNext Application</span>
                </div>
                <span className="text-sm text-muted-foreground">Operational</span>
              </div>
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <div className="h-3 w-3 rounded-full bg-green-500" />
                  <span>Database</span>
                </div>
                <span className="text-sm text-muted-foreground">Operational</span>
              </div>
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <div className="h-3 w-3 rounded-full bg-green-500" />
                  <span>Background Jobs</span>
                </div>
                <span className="text-sm text-muted-foreground">Operational</span>
              </div>
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <div className="h-3 w-3 rounded-full bg-green-500" />
                  <span>Email Services</span>
                </div>
                <span className="text-sm text-muted-foreground">Operational</span>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <div className="flex items-center justify-between">
              <div>
                <CardTitle>Recent Invoices</CardTitle>
                <CardDescription>Your payment history</CardDescription>
              </div>
              <Button variant="outline" size="sm" asChild>
                <Link href="/billing">View All</Link>
              </Button>
            </div>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {recentInvoices.map((invoice) => (
                <div key={invoice.id} className="flex items-center justify-between">
                  <div>
                    <div className="font-medium">{invoice.id}</div>
                    <div className="text-sm text-muted-foreground">{invoice.date}</div>
                  </div>
                  <div className="text-right">
                    <div className="font-medium">{invoice.amount}</div>
                    <Badge variant="secondary" className="text-xs">
                      {invoice.status}
                    </Badge>
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  )
}
