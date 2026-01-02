'use client'

import Link from 'next/link'
import { usePathname } from 'next/navigation'
import { cn } from '@/lib/utils'
import {
  LayoutDashboard,
  Building2,
  Users,
  Package,
  CreditCard,
  Settings,
  BarChart3,
  Server,
  Bell,
  FileText,
  HelpCircle,
  LogOut,
} from 'lucide-react'
import { Button } from '@/components/ui/button'
import { ScrollArea } from '@/components/ui/scroll-area'
import { Separator } from '@/components/ui/separator'

const menuItems = [
  {
    title: 'Overview',
    items: [
      { title: 'Dashboard', href: '/dashboard', icon: LayoutDashboard },
      { title: 'Analytics', href: '/analytics', icon: BarChart3 },
    ],
  },
  {
    title: 'Management',
    items: [
      { title: 'Tenants', href: '/tenants', icon: Building2 },
      { title: 'Users', href: '/users', icon: Users },
      { title: 'Modules', href: '/modules', icon: Package },
      { title: 'Subscriptions', href: '/subscriptions', icon: CreditCard },
    ],
  },
  {
    title: 'Infrastructure',
    items: [
      { title: 'Containers', href: '/containers', icon: Server },
      { title: 'Notifications', href: '/notifications', icon: Bell },
      { title: 'Logs', href: '/logs', icon: FileText },
    ],
  },
  {
    title: 'System',
    items: [
      { title: 'Settings', href: '/settings', icon: Settings },
      { title: 'Help', href: '/help', icon: HelpCircle },
    ],
  },
]

export function Sidebar() {
  const pathname = usePathname()

  return (
    <div className="flex h-screen w-64 flex-col border-r bg-card">
      {/* Logo */}
      <div className="flex h-16 items-center border-b px-6">
        <Link href="/dashboard" className="flex items-center gap-2">
          <div className="flex h-8 w-8 items-center justify-center rounded-lg bg-primary text-primary-foreground">
            <Package className="h-5 w-5" />
          </div>
          <span className="font-bold">ERPNext SaaS</span>
        </Link>
      </div>

      {/* Navigation */}
      <ScrollArea className="flex-1 px-3 py-4">
        <nav className="space-y-6">
          {menuItems.map((section) => (
            <div key={section.title}>
              <h4 className="mb-2 px-3 text-xs font-semibold uppercase text-muted-foreground">
                {section.title}
              </h4>
              <div className="space-y-1">
                {section.items.map((item) => (
                  <Link key={item.href} href={item.href}>
                    <Button
                      variant={pathname === item.href ? 'secondary' : 'ghost'}
                      className={cn(
                        'w-full justify-start',
                        pathname === item.href && 'bg-secondary'
                      )}
                    >
                      <item.icon className="mr-2 h-4 w-4" />
                      {item.title}
                    </Button>
                  </Link>
                ))}
              </div>
            </div>
          ))}
        </nav>
      </ScrollArea>

      {/* Footer */}
      <div className="border-t p-4">
        <Button variant="ghost" className="w-full justify-start text-muted-foreground">
          <LogOut className="mr-2 h-4 w-4" />
          Logout
        </Button>
      </div>
    </div>
  )
}
