'use client'

import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from 'recharts'

const data = [
  { module: 'Accounting', tenants: 127 },
  { module: 'CRM', tenants: 98 },
  { module: 'Stock', tenants: 85 },
  { module: 'HR', tenants: 72 },
  { module: 'Selling', tenants: 68 },
  { module: 'Healthcare', tenants: 45 },
  { module: 'Education', tenants: 38 },
  { module: 'Manufacturing', tenants: 32 },
]

export function ModuleUsageChart() {
  return (
    <ResponsiveContainer width="100%" height={350}>
      <BarChart data={data} layout="vertical">
        <CartesianGrid strokeDasharray="3 3" className="stroke-muted" horizontal={false} />
        <XAxis
          type="number"
          stroke="hsl(var(--muted-foreground))"
          fontSize={12}
          tickLine={false}
          axisLine={false}
        />
        <YAxis
          dataKey="module"
          type="category"
          stroke="hsl(var(--muted-foreground))"
          fontSize={12}
          tickLine={false}
          axisLine={false}
          width={100}
        />
        <Tooltip
          content={({ active, payload }) => {
            if (active && payload && payload.length) {
              return (
                <div className="rounded-lg border bg-background p-2 shadow-sm">
                  <div className="flex flex-col">
                    <span className="text-[0.70rem] uppercase text-muted-foreground">
                      {payload[0].payload.module}
                    </span>
                    <span className="font-bold">
                      {payload[0].value} tenants
                    </span>
                  </div>
                </div>
              )
            }
            return null
          }}
        />
        <Bar
          dataKey="tenants"
          fill="hsl(var(--primary))"
          radius={[0, 4, 4, 0]}
        />
      </BarChart>
    </ResponsiveContainer>
  )
}
